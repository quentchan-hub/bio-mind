using Godot;
using System;


public partial class GameScreen : Control
{
	
	[Signal] public delegate void OnRowCompleteEventHandler(Godot.Collections.Array<int> playerGuess);

	
	[Export] Brain Brain;
	[Export] Label LabelLevelValue;
	[Export] Label LabelTryValue;
	[Export] Label LabelTryMaxValue;
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
	
	
	// Ready
	public override void _Ready()
	{
		//_boardButtonFocused = rowInstance.btnFocused;
		Brain.UpdateCurrentRow += DisplayCurrentTryValue;
		Brain.OnHintsReady += ManageHints;
	}
	
	// Process
	public override void _Process(double delta)
	{}
	
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
		
		GD.Print("test gamescreen -> rows : " + rows);
		
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
		GD.Print("colors = " + colors);
		if (colors == 4)
		{
			ColorKeyboard.Columns = 4;
		}
		if (colors <= 3)
		{
			ColorKeyboard.Columns = colors;
		}
		//else { ColorKeyboard.Columns = 3; }
		GD.Print("nb de colonne = " + ColorKeyboard.Columns);
	}

	
	public void FirstButtonFocus()
	{
		// joue une methode de RowAttempt pour Focus premier bouton du board
		_firstRow.CallDeferred("FocusFirstInRow");
		// Force le scroll a commencer tout en haut
		scrollContainer.ScrollVertical = 0;
		// Recupere en tant que _boardButtonFocused le bouton qui a le focus (FirstButton)
		//_boardButtonFocused = GetViewport().GuiGetFocusOwner() as BoardButton;
	}
	
	public void OnBoardButtonFocused(BoardButton button)
	{
		_boardButtonFocused = button;
	}
	public void OnFocusedRow(AttemptRow row)
	{
		FocusedRow = row;
	}
	
	//  == MISE EN RELATION COULEUR SELECTIONNEE/APPLIQUEE == //
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
	
	private void FocusNextButton()
	{
		// On automatise la passation de focus au prochain bouton(1)
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
	
	// AFFICHAGE AIDE DE JEU
	private void _on_help_button_pressed()
	{
		TutorialOverlay.Visible = true;
	}
	
	// APPUI SUR VALIDER => Envoi selection a Brain
	public void _on_validate_button_pressed()
	{
		var playerSlotContainer = _boardButtonFocused.GetParent().GetParent();
		
		// RAZ playerGuess entre chaque ligne
		playerGuess.Clear();
		
		foreach (Node child in playerSlotContainer.GetChildren())
		{
			foreach (Node button in child.GetChildren())
			{
				if (button is BoardButton boardButton)
				{
					// d'abord on s'assure que toute la ligne est remplie
					if (boardButton.Icon == null)
					{
						// sinon on affiche un warning et return
						WarningLabels.EmptySlotWarning();
						return;
					}
					else 
					{
						// si oui on recupere index des couleurs et on stocke dans playerGuess
						int index = Array.IndexOf(boardButton.HeadTextures, boardButton.Icon);
						playerGuess.Add(index);
					}
					
				}
			}
		}
		
		// Facile : refuse les doublons de couleur
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
		// Ensuite il y a l'envoi a la logique de jeu GameLogic
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

		// Hints rouges (bien places)
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
		
		// Hints blancs (mal places)
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
	
	public void NextRow(int _currentRow)
	{
		// car currentRow  = nextRow index
		AttemptRow nextRow = RowContainer.GetChild(_currentRow) as AttemptRow; 
		nextRow.FocusFirstInRow();
	}
	
	// MAJ Compteur "Essai"
	private void DisplayCurrentTryValue(int row)
	{
		int current = row + 1;
		LabelTryValue.Text = current.ToString();
		GD.Print($"Essai {current}");
	}
	

}
