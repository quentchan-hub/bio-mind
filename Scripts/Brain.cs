using Godot;
using System;

public partial class Brain : Node
{
	[Signal]
	public delegate void DisplaySolutionEventHandler(Godot.Collections.Array<int> _solution);
	
	[Signal]
	public delegate void OnHintsReadyEventHandler(int rightPosition, int wrongPosition);
	
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
	private int _currentRow = 0;
	private int _gameDifficulty = 1;

	private Godot.Collections.Array<int> _solution = new();

	public override void _Ready()
	{
		_on_easy_btn_pressed();
		GameScreen.OnRowComplete += CheckRow;
	}

	// ================= CONFIGURATION =================

	private void GameConfigurator(string difficulty, int rows, int slots, int hints)
	{
		GameScreen.DisplayLevel(difficulty);
		GameScreen.InstantiateBoard(rows, slots, hints);
		GameScreen.DisplayMaxTry(rows.ToString());
		EndScreen.InstantiateResultSlots(slots);

		_rows = rows;
		_slots = slots;
		_hints = hints;
		_currentRow = 0;

		GameScreen.FirstButtonFocus();
		EmitSignal(SignalName.UpdateCurrentRow, _currentRow);

		NewGame();
	}

	private void _on_easy_btn_pressed()
	{
		GameConfigurator("Facile", 12, 4, 4);
		_gameDifficulty = 1;
	}

	private void _on_medium_btn_pressed()
	{
		GameConfigurator("Moyen", 10, 4, 4);
		_gameDifficulty = 2;
	}

	private void _on_expert_btn_pressed()
	{
		GameConfigurator("Expert", 8, 4, 4);
		_gameDifficulty = 3;
	}

	private void _on_hard_btn_pressed()
	{
		GameConfigurator("Hard", 6, 5, 5);
		_gameDifficulty = 4;
	}
	
	private void _on_play_again_pressed()
	{
		switch (_gameDifficulty)
		{
			case 1: _on_easy_btn_pressed(); break;
 			case 2: _on_medium_btn_pressed(); break;
			case 3: _on_expert_btn_pressed(); break;
			case 4: _on_hard_btn_pressed(); break;
		}

	}
	
	// ================= NOUVELLE PARTIE =================

	private void NewGame()
	{
		RandomPickedNumbers();
	}

	private void RandomPickedNumbers()
	{
		_solution.Clear();

		for (int i = 0; i < _slots; i++)
		{
			int randomNb = (int)(GD.Randi() % 6);
			_solution.Add(randomNb);
		}

		GD.Print("CODE : " + _solution);
	}

	// ================= CHECK =================

	private void CheckRow(Godot.Collections.Array<int> playerGuess)
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
		GD.Print("right "+ rightPosition, " wrong "+ wrongPosition);
		GD.Print("emission signal hints");
		EmitSignal(SignalName.OnHintsReady, rightPosition, wrongPosition);

		if (rightPosition == _slots)
		{
			EmitSignal(SignalName.OnGameOver, true); // partie gagné
			EmitSignal(SignalName.DisplaySolution, _solution);
			return;
		}

		if (_currentRow >= _rows - 1)
		{
			EmitSignal(SignalName.OnGameOver, false); // partie perdue
			EmitSignal(SignalName.DisplaySolution, _solution);
			return;
		}

		_currentRow++;
		EmitSignal(SignalName.UpdateCurrentRow, _currentRow);
		GameScreen.NextRow(_currentRow);
	}
}
