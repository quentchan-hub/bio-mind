using Godot;
using System;


public partial class GameScreen : Control
{
	[Signal] 
	public delegate void ChronoTimeEventHandler(int chronoElapsed, int difficulty);

	[Signal] 
	public delegate void OnRowCompleteEventHandler(Godot.Collections.Array<int> playerGuess);
	
	[Export] Brain Brain;
	[Export] Label LabelLevelValue;
	[Export] Label LabelTryValue;
	[Export] Label LabelTryMaxValue;
	[Export] Label LabelChronoValue; 
	[Export] VBoxContainer RowContainer;
	[Export] PackedScene AttemptRowScene;
	[Export] ScrollContainer scrollContainer;
	[Export] Control TutorialOverlay;
	[Export] WarningLabels WarningLabels;
	[Export] AudioStreamPlayer HintAudio;
	[Export] GridContainer ColorKeyboard;
	
	// Facile : interdit les doublons de couleur dans une ligne
	public bool AllowDuplicateColors = true;
	
	private AttemptRow rowInstance;
	private AttemptRow _firstRow;
	private BoardButton _boardButtonFocused;
	private BoardButton _button;
	private int _slots;
	private AttemptRow FocusedRow;
	
	public Godot.Collections.Array<int> playerGuess = new();
	
	// Chrono
	private float _chronoElapsed = 0f;
	private bool  _chronoRunning = false;
	private bool  _chronoEnabled = false;
	private int   _currentDifficulty = 1;
	
	// ============================================================
	// READY / PROCESS
	// ============================================================
	
	public override void _Ready()
	{
		Brain.OnGameOver += OnGameOver;
		Brain.UpdateCurrentRow += DisplayCurrentTryValue;
		Brain.OnHintsReady += ManageHints;
	}
	
	public override void _Process(double delta)
	{
		if (!_chronoRunning) return;

		_chronoElapsed += (float)delta;

		if (LabelChronoValue != null)
		{
			int minutes = (int)(_chronoElapsed / 60);
			int seconds = (int)(_chronoElapsed % 60);
			LabelChronoValue.Text = $"{minutes:00}:{seconds:00}";
		}
	}

	// ============================================================
	// CHRONO
	// ============================================================

	// Connecté via l'inspecteur au signal toggled du CheckButton
	private void _on_check_button_toggled(bool toggledOn)
	{
		_chronoEnabled = toggledOn;
	}

	public void StartChrono()
	{
		if (!_chronoEnabled) return;

		_chronoElapsed = 0f;
		_chronoRunning = true;

		if (LabelChronoValue != null)
			LabelChronoValue.Text = "00:00";
	}

	public void StopChrono()
	{
		if (!_chronoEnabled) return;

		_chronoRunning = false;

		int elapsed = (int)_chronoElapsed;
		EmitSignal(SignalName.ChronoTime, elapsed, _currentDifficulty);

		GD.Print($"Chrono arrêté : {elapsed}s  difficulté : {_currentDifficulty}");
	}

	private void OnGameOver(bool victory)
	{
		if (victory) StopChrono();
		else {_chronoRunning = false;}
	}

	// ============================================================
	// CONFIGURATION
	// ============================================================
	
	public void SetDifficulty(int difficulty)
	{
		_currentDifficulty = difficulty;
	}
	
	public void DisplayLevel(string difficulty)
	{
		LabelLevelValue.Text = difficulty;
	}
	
	public void DisplayMaxTry(string maxTry)
	{
		LabelTryMaxValue.Text = maxTry;
	}
	
	public void InstantiateBoard(int rows, int slots, int hints)
	{
		_slots = slots;
		
		// RAZ
		foreach (Node child in RowContainer.GetChildren())
		{
			RowContainer.RemoveChild(child);
			child.QueueFree();
		}
		
		for (int i = 0; i < rows; i++)
		{
			var rowInstance = AttemptRowScene.Instantiate() as AttemptRow;
			RowContainer.AddChild(rowInstance);

			rowInstance.UpdateSlots(slots);
			rowInstance.UpdateHints(hints);	
			
			rowInstance.OnBoardButtonFocused += OnBoardButtonFocused;
			rowInstance.OnFocusedRow += OnFocusedRow;
		}
		
		_firstRow = RowContainer.GetChild(0) as AttemptRow;
	}
	
	public void DisplayColors(int colors)
	{
		ColorKeyboard.Columns = 3;
		int lastIndex = ColorKeyboard.GetChildCount() - 1;
		foreach (Node child in ColorKeyboard.GetChildren())
		{
			if (child is Button color)
			{
				color.Visible = true;
			}
		}
		for (int i = lastIndex; i >= colors; i--)
		{
			if (ColorKeyboard.GetChild(i) is Button color)
			{
				color.Visible = false;
			}
		}
		//GD.Print("colors = " + colors);
		if (colors == 4)
		{
			ColorKeyboard.Columns = 4;
		}
		if (colors <= 3)
		{
			ColorKeyboard.Columns = colors;
		}
		//GD.Print("nb de colonne = " + ColorKeyboard.Columns);
	}

	// ============================================================
	// FOCUS / NAVIGATION
	// ============================================================
	
	public void FirstButtonFocus()
	{
		_firstRow.CallDeferred("FocusFirstInRow");
		scrollContainer.ScrollVertical = 0;
	}
	
	public void OnBoardButtonFocused(BoardButton button)
	{
		_boardButtonFocused = button;
	}

	public void OnFocusedRow(AttemptRow row)
	{
		FocusedRow = row;
	}
	
	private void FocusNextButton()
	{
		var playerSlotContainer = _boardButtonFocused.GetParent().GetParent();
		var currentSlotIndex = _boardButtonFocused.GetParent().GetIndex();
		if (currentSlotIndex < _slots - 1)
		{
			int nextSlotIndex = currentSlotIndex + 1;
			var nextPlayerSlot = playerSlotContainer.GetChild(nextSlotIndex);
			var nextBoardButton = nextPlayerSlot.GetChild(0) as BoardButton;
			nextBoardButton.GrabFocus();
			_boardButtonFocused = nextBoardButton;
		}
	}

	// ============================================================
	// COULEURS
	// ============================================================
	
	private void _on_head_button_black_pressed()
	{
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.Black);
		FocusNextButton();
	}
	private void _on_head_button_blue_pressed()
	{
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.Blue);
		FocusNextButton();
	}
	private void _on_head_button_purple_pressed()
	{
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.Purple);
		FocusNextButton();
	}
	private void _on_head_button_red_pressed()
	{
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.Red);
		FocusNextButton();
	}	
	private void _on_head_button_white_pressed()
	{
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.White);
		FocusNextButton();
	}
	private void _on_head_button_yellow_pressed()
	{
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.Yellow);
		FocusNextButton();
	}

	// ============================================================
	// AIDE DE JEU
	// ============================================================
	
	private void _on_help_button_pressed()
	{
		TutorialOverlay.Visible = true;
	}

	// ============================================================
	// VALIDER / EFFACER
	// ============================================================
	
	public void _on_validate_button_pressed()
	{
		var playerSlotContainer = _boardButtonFocused.GetParent().GetParent();
		
		playerGuess.Clear();
		
		foreach (Node child in playerSlotContainer.GetChildren())
		{
			foreach (Node button in child.GetChildren())
			{
				if (button is BoardButton boardButton)
				{
					if (boardButton.Icon == null)
					{
						WarningLabels.EmptySlotWarning();
						return;
					}
					else 
					{
						int index = Array.IndexOf(boardButton.HeadTextures, boardButton.Icon);
						playerGuess.Add(index);
					}
				}
			}
		}
		
		if (!AllowDuplicateColors)
		{
			var uniq = new System.Collections.Generic.HashSet<int>(playerGuess);
			if (uniq.Count != playerGuess.Count)
			{
				WarningLabels.NoDoubleColorWarning();
				return;
			}
		}
		FocusedRow.StopAnim();
		EmitSignal(SignalName.OnRowComplete, playerGuess);
	}
	
	private void _on_clear_button_pressed()
	{
		var playerSlotContainer = _boardButtonFocused.GetParent().GetParent();
		foreach (Node child in playerSlotContainer.GetChildren())
		{
			foreach (Node button in child.GetChildren())
			{
				if (button is BoardButton boardButton)
				{
					boardButton.ClearIcon();
				}
			}
		}
		var _firstButton = playerSlotContainer.GetChild(0).GetChild(0) as BoardButton;
		_firstButton.GrabFocus();
		_firstButton.AcceptEvent();
		OnBoardButtonFocused(_firstButton);
	}

	// ============================================================
	// HINTS
	// ============================================================
	
	public async void ManageHints(int rightPosition, int wrongPosition)
	{
		var currentHintContainer = FocusedRow.FindChild("HintContainer*");
		Godot.Collections.Array<HintButton> hintButtons = new();
		foreach (Node child in currentHintContainer.GetChildren())
		{
			if (child is HintButton hint)
			{
				hintButtons.Add(hint);
			}
		}
		
		int hintIndex = 0;

		for (int i = 0; i < rightPosition; i++)
		{
			if (hintIndex < hintButtons.Count)
			{
				(hintButtons[hintIndex] as HintButton)?.RedHint();
				HintAudio.Play();
				hintIndex++;
				await ToSignal(GetTree().CreateTimer(0.5), "timeout");
			}
		}
		
		for (int i = 0; i < wrongPosition; i++)
		{
			if (hintIndex < hintButtons.Count)
			{
				(hintButtons[hintIndex] as HintButton)?.WhiteHint();
				HintAudio.Play();
				hintIndex++;
				await ToSignal(GetTree().CreateTimer(0.5), "timeout");
			}
		}
	}

	// ============================================================
	// LABELS
	// ============================================================
	
	public void NextRow(int _currentRow)
	{
		AttemptRow nextRow = RowContainer.GetChild(_currentRow) as AttemptRow; 
		nextRow.FocusFirstInRow();
	}
	
	private void DisplayCurrentTryValue(int row)
	{
		int current = row + 1;
		LabelTryValue.Text = current.ToString();
		//GD.Print($"Essai {current}");
	}
}
