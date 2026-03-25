using Godot;
using System;

public partial class Brain : Node
{
	[Signal]
	public delegate void DisplaySolutionEventHandler(Godot.Collections.Array<int> _solution);
	
	[Signal]
	public delegate void OnHintsReadyEventHandler(int rightPosition, int wrongPosition);
	
	[Signal]
	public delegate void OnAccountingGamePlayedEventHandler(int _gameDifficulty, bool _victory);
	
	[Signal]
	public delegate void UpdateCurrentRowEventHandler(int currentRow);

	[Signal]
	public delegate void OnGameOverEventHandler(bool victory);

	[Export] HomeScreen HomeScreen;
	[Export] GameScreen GameScreen;
	[Export] EndScreen EndScreen;
	[Export] UiManager UiManager;

	private int _slots = 4;
	private int _hints = 4;
	private int _rows = 12;
	private int _colors = 6;
	private int _currentRow = 0;
	private int _gameDifficulty = 1;

	public Godot.Collections.Array<int> _solution = new();

	public override void _Ready()
	{
		GameScreen.OnRowComplete += CheckRow;
		CallDeferred(nameof(InitGame));
	}

	private void InitGame()
	{
		_on_easy_btn_pressed();
	}

	// ================= CONFIGURATION =================

	private void GameConfigurator(string difficulty, int rows, int slots, int hints, int colors)
	{
		GameScreen.DisplayLevel(difficulty);
		GameScreen.InstantiateBoard(rows, slots, hints);
		GameScreen.DisplayMaxTry(rows.ToString());
		GameScreen.DisplayColors(colors);
		EndScreen.InstantiateResultSlots(slots);
		EndScreen.DifficultyMode(_gameDifficulty);
		GameScreen.SetDifficulty(_gameDifficulty);
		
		_rows = rows;
		_slots = slots;
		_hints = hints;
		_colors = colors;
		_currentRow = 0;

		// Facile : interdit les doublons de couleur
		GameScreen.AllowDuplicateColors = _gameDifficulty != 1;

		EmitSignal(SignalName.UpdateCurrentRow, _currentRow);
	}

	private void _on_easy_btn_pressed()
	{
		_gameDifficulty = 1;
		GameConfigurator(difficulty: "Facile", rows: 12, slots: 3, hints: 4, colors: 4);
	}

	private void _on_medium_btn_pressed()
	{
		_gameDifficulty = 2;
		GameConfigurator(difficulty: "Moyen", rows: 10, slots: 4, hints: 4, colors: 6);
	}

	private void _on_hard_btn_pressed()
	{
		_gameDifficulty = 3;
		GameConfigurator(difficulty: "Difficile", rows: 8, slots: 4, hints: 4, colors: 6);
	}

	private void _on_expert_btn_pressed()
	{
		_gameDifficulty = 4;
		GameConfigurator(difficulty: "Expert", rows: 8, slots: 5, hints: 5, colors: 6);
	}
	
	private void _on_play_btn_pressed()
	{
		switch (_gameDifficulty)
		{
			case 1: _on_easy_btn_pressed(); break;
 			case 2: _on_medium_btn_pressed(); break;
			case 3: _on_hard_btn_pressed(); break;
			case 4: _on_expert_btn_pressed(); break;
		}
		NewGame();
	}
	
	private void _on_play_again_pressed()
	{
		switch (_gameDifficulty)
		{
			case 1: _on_easy_btn_pressed(); break;
 			case 2: _on_medium_btn_pressed(); break;
			case 3: _on_hard_btn_pressed(); break;
			case 4: _on_expert_btn_pressed(); break;
		}
		NewGame();
	}
	
	// ================= NOUVELLE PARTIE =================

	private void NewGame()
	{
		RandomPickedNumbers();
		UiManager.OnGameStart();
	}

	private void RandomPickedNumbers()
	{
		_solution.Clear();

		if (_gameDifficulty == 1)
		{
			// Facile : tire sans repetition
			var pool = new System.Collections.Generic.List<int>();
			for (int i = 0; i < _colors; i++) pool.Add(i);

			for (int i = pool.Count - 1; i > 0; i--)
			{
				int j = (int)(GD.Randi() % (uint)(i + 1));
				int tmp = pool[i];
				pool[i] = pool[j];
				pool[j] = tmp;
			}

			for (int i = 0; i < _slots; i++)
			{
				_solution.Add(pool[i]);
			}
		}
		else
		{
			for (int i = 0; i < _slots; i++)
			{
				int randomNb = (int)(GD.Randi() % (uint)_colors); // Randi renvoi un type uint = entier positif seulement
				_solution.Add(randomNb);
			}
		}

		GD.Print("CODE : " + _solution);
	}

	// ================= CHECK =================

	public void CheckRow(Godot.Collections.Array<int> playerGuess)
	{
		int rightPosition = 0;
		int wrongPosition = 0;

		var pendingSolution = new Godot.Collections.Array<int>(_solution);

		for (int i = 0; i < playerGuess.Count; i++)
		{
			if (playerGuess[i] == _solution[i])
			{
				rightPosition++;
				pendingSolution[i] = -1;
			}
		}
	
		for (int i = 0; i < playerGuess.Count; i++)
		{
			if (playerGuess[i] != _solution[i] && pendingSolution.Contains(playerGuess[i]))
			{
				wrongPosition++;
				int index = pendingSolution.IndexOf(playerGuess[i]);
				pendingSolution[index] = -1;
			}
		}
		//GD.Print("right "+ rightPosition, " wrong "+ wrongPosition);
		EmitSignal(SignalName.OnHintsReady, rightPosition, wrongPosition);

		if (rightPosition == _slots)
		{
			EmitSignal(SignalName.OnAccountingGamePlayed, _gameDifficulty, true);
			EmitSignal(SignalName.OnGameOver, true); // partie gagne
			EmitSignal(SignalName.DisplaySolution, _solution);
			return;
		}

		if (_currentRow >= _rows - 1)
		{
			EmitSignal(SignalName.OnAccountingGamePlayed, _gameDifficulty, false);
			EmitSignal(SignalName.OnGameOver, false); // partie perdue
			EmitSignal(SignalName.DisplaySolution, _solution);
			return;
		}

		_currentRow++;
		EmitSignal(SignalName.UpdateCurrentRow, _currentRow);
		GameScreen.NextRow(_currentRow);
	}
	
	
	
	//===============================================================================
	// FIN DU SCRIPT
	//===============================================================================
	
}
