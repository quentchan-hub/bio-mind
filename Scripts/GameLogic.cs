using Godot;
using System;

public partial class GameLogic : Node
{
	[Signal] 
	public delegate void OnHintsReadyEventHandler(int rightPosition, int wrongPosition);
	[Signal] 
	public delegate void OnAttemptCountChangedEventHandler(int currentAttempt, int maxAttempts);
	[Signal] 
	public delegate void OnGameOverEventHandler(bool victory);
	
	[Export] GameZone GameZone;
	
	// si c'est la première partie le joueur n'a pas le choix de la difficulté
	// et commence par le tuto ce qui signifie : 
	// 3 couleur à trouver, 12 essais possible et des explications de règle.
	private bool firstGame = false;
	
	private Godot.Collections.Array<int> _solution = new();
	private int _numberCount = 4;
	private int _randomNumber;
	private int rightPosition = 0;
	private int wrongPosition = 0;
	private int _currentAttempt = 1;
	private int _maxAttempts = 12;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameZone.OnPlayerGuessComplete += CheckRow;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	
	
	// Lors de la 1ère partie par défaut ou si coché dans option
	
	private void StartTutorial()
	{
		_numberCount = 3;  					// 3 couleurs à trouver
		RandomPickedNumbers(_numberCount);
	}
	
	// Lors des parties suivantes
	
	private void StartGame()
	{
		_numberCount = 4;					// 4 couleurs à trouver
		RandomPickedNumbers(_numberCount);
	}

	
	// La création de la solution au hasard se passe ici
	
	private void RandomPickedNumbers(int numberCount)
	{
		
		// RAZ de la solution précédente s'il y a
		_solution.Clear();
			
		// Nouvelle solution	
		for (int i = 0; i < numberCount; i++)
		{
			_randomNumber = (int)(GD.Randi() % 6); 	
			// 6 = nombre de couleurs (entre 0 et 5), cast (int) car GD.Randi en uint
			// uint = que des nombres int positifs, entre 0 et 2^32 - 1
			
			_solution.Add(_randomNumber);
		}
		
		GD.Print("CODE CACHE A DECOUVRIR : " + _solution);
	}
		
	// quand le bouton Play est pressé la partie commence
	
	private void _on_play_btn_pressed()
	{
		if (firstGame == true)
			StartTutorial();
		
		else 
			StartGame();
	}
	
	
	// Quand on presse rejouer ça repart sur la même difficulté que précédemment
	
	private void _on_play_again_pressed()
	{
		StartGame();
	}
	
	
	// Lorsque le bouton Valider est pressé le jeu doit comparer son set  
	// de couleur et celui du joueur
	
	private void CheckRow(Godot.Collections.Array<int> playerGuess)
	{
		rightPosition = 0;
		wrongPosition = 0;

		// On crée une copie de solution pour la suite
		Godot.Collections.Array<int> pendingSolution = new();
		for (int i = 0; i < _solution.Count; i++)
		{
			pendingSolution.Add(_solution[i]);
		}
		
		// 1ère passe - bonnes positions ET on les marque comme comptés
		for (int i = 0; i < playerGuess.Count; i++)
		{
			if (playerGuess[i] == _solution[i])
			{
				rightPosition++;
				pendingSolution[i] = -1;  // -1 = ignorés pour la suite
			}
		}
		
		// 2e passe - mauvaises positions (sur ce qui reste)
		for (int i = 0; i < playerGuess.Count; i++)
		{
			if (playerGuess[i] != _solution[i] && pendingSolution.Contains(playerGuess[i]))
			{
				wrongPosition++;
				int index = pendingSolution.IndexOf(playerGuess[i]);
				pendingSolution[index] = -1;
			}
		}
		
	
		// Transmettre le bilan à GameZone pour qu'il gère l'affichage des hints
		
		EmitSignal(SignalName.OnHintsReady, rightPosition, wrongPosition);
		
		// Si gagné
		if (rightPosition == _numberCount)
		{
			EmitSignal(SignalName.OnGameOver, true);  // victory = true
			return;
		}
		
		// Si max atteint
		if (_currentAttempt >= _maxAttempts)
		{
			EmitSignal(SignalName.OnGameOver, false);  // victory = false
			return;
		}
		
		// Sinon on passe à la ligne suivante (et NextRow gère si pas de ligne suivante)
		GameZone.NextRow();
			
		// Envoyer la mise à jour du compteur
		_currentAttempt++;
		EmitSignal(SignalName.OnAttemptCountChanged, _currentAttempt, _maxAttempts);
	}
	
}
