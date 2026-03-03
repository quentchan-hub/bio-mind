using Godot;
using System;
using System.Collections.Generic;

public partial class GameZone : Control
{
	[Signal] 
	public delegate void OnPlayerGuessCompleteEventHandler(Godot.Collections.Array<int> playerGuess);
	
	[Export] private GameLogic GameLogic; 
	[Export] private Label LabelNbTentative;
	[Export] private Label LabelNbMax;
	[Export] private Label LabelNiveauSel;
	[Export] private BoardButton _firstBoardButton;
	[Export] private AttemptRow AttemptRow1;
	[Export] private AttemptRow AttemptRow7;
	[Export] private AttemptRow AttemptRow8;
	[Export] private AttemptRow AttemptRow9;
	[Export] private AttemptRow AttemptRow10;
	[Export] private AttemptRow AttemptRow11;
	[Export] private AttemptRow AttemptRow12;
	[Export] private WarningLabels WarningLabels;
	
	private BoardButton _boardButtonFocused;
	private AttemptRow _currentRow;
	private AttemptRow _nextRow;
	private int _playerSlotCount = 4;
	private int _gameSlotCount = 4;
	public Godot.Collections.Array<int> playerGuess = new();
	
	
	public override void _Ready()
	{
		GameLogic.OnHintsReady += ManageHints;
		GameLogic.OnAttemptCountChanged += UpdateAttemptLabel;
	}
		
		//  == GESTION DES FOCUS BOUTONS  == //
	
	public void _on_board_button_focus_entered()
	{
		// Donne le Focus au bouton pressed
		_boardButtonFocused = GetViewport().GuiGetFocusOwner() as BoardButton;
	}
	
	public void _on_play_btn_pressed()
	{
		// On donne automatiquement au 1er Bouton le Focus au début du jeu
		_firstBoardButton.GrabFocus();
		// On force l'affichage visuel du Focus
		_firstBoardButton.AcceptEvent();
		// On donne du feedback visuel sur la ligne selectionnée
		AttemptRow1.GetTheRowGlow();
	}
	
	private void FocusNextButton()
	{
		// On automatise la passation de focus au prochain bouton(1)
		var playerSlotContainer = _boardButtonFocused.GetParent().GetParent();
		var currentSlotIndex = _boardButtonFocused.GetParent().GetIndex();
		if (currentSlotIndex < _playerSlotCount - 1)
		{
			int nextSlotIndex = currentSlotIndex + 1;
			var nextPlayerSlot = playerSlotContainer.GetChild(nextSlotIndex);
			var nextBoardButton = nextPlayerSlot.GetChild(0) as BoardButton;
			nextBoardButton.GrabFocus();
			_boardButtonFocused = nextBoardButton;
		}
	}
	
		
		//  == MISE EN RELATION COULEUR SELECTIONNEE/APPLIQUEE == //
	private void _on_head_button_blue_pressed()
	{				
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.Blue);
		FocusNextButton();
	}
	private void _on_head_button_black_pressed()
	{
		_boardButtonFocused?.SetHead((int)BoardButton.HeadColor.Black);
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
	
		 //  == GESTION VISIBILITE ROWS == //
		
		
	// à voir mais peut être intéressant pour la durée de vie de faire une 
	// première partie en ultra facile avec seulement 3 couleurs à trouver
	// en mode tuto qui prend en main.
	// et ensuite lorsque le joueur a gagné la première partie on lui indique que 
	// ouah il a débloqué un slot supplémentaire arrivera t'il à relever le défi ?!
	// et ça part en mode facile. 
	// donc au ready : 12 lignes // 3 couleurs à trouver
	// donc 3 slots réponse ordi
	
	// _on_tuto_btn_button_down()
//	{
		// 12 lignes visibles
		// 4 couleurs à trouver
		//_playerSlotCount = 4;
		// 4 slots réponse ordi
		//_gameSlotCount = 4
		// envoi signal à LabelEssaiMax => joueur a 12 essais
		// envoi signal à game logic => joueur a 12 essais
//	}
		
	private void _on_easy_btn_pressed()
	{
		// Afficher le mode de difficulté
		LabelNiveauSel.Text = "Facile";
		// 12 lignes visibles
		LabelNbMax.Text = "12";
		// 4 couleurs à trouver
		_playerSlotCount = 4;
		// 4 slots réponse ordi
		_gameSlotCount = 4;
		// envoi signal à LabelEssaiMax => joueur a 12 essais
		// envoi signal à game logic => joueur a 12 essais
	}

	private void _on_medium_btn_pressed()
	{
		// Afficher le mode de difficulté
		LabelNiveauSel.Text = "Moyen";
		// 10 lignes visibles (donc on enlève row 11 et 12) 
		AttemptRow11.Visible = false;
		AttemptRow12.Visible = false;
		// 10 essais max
		LabelNbMax.Text = "10";
		// 4 couleurs à trouver
		_playerSlotCount = 4;
		// 4 slots réponse ordi
		_gameSlotCount = 4;
	
		// envoi signal à game logic => joueur a 10 essais
	}

	private void _on_expert_btn_pressed()
	{
		// Afficher le mode de difficulté
		LabelNiveauSel.Text = "Expert";
		// 8 lignes visibles (donc les 4 dernières invisibles)
		AttemptRow9.Visible = false;
		AttemptRow10.Visible = false;
		AttemptRow11.Visible = false;
		AttemptRow12.Visible = false;
		// 8 essais max
		LabelNbMax.Text = "8";
		// 4 couleurs à trouver
		_playerSlotCount = 4;
		// 4 slots réponse ordi
		_gameSlotCount = 4;

		// envoi signal à game logic => joueur a 8 essais
		// dévoile une gemme => si joueur obtient 5 gemmes ...
		// ...=> coffre entièrement ouvert => dévoile hardcore mode
	}

	private void _on_hard_btn_pressed()
	{
		// Afficher le mode de difficulté
		LabelNiveauSel.Text = "HardCore";
		// 6 lignes visibles (donc les 6 dernières invisibles)
		AttemptRow7.Visible = false;
		AttemptRow8.Visible = false;
		AttemptRow9.Visible = false;
		AttemptRow10.Visible = false;
		AttemptRow11.Visible = false;
		AttemptRow12.Visible = false;
		// 6 essais max
		LabelNbMax.Text = "6";
		// 5 couleurs à trouver
		_playerSlotCount = 4;
		// 5 slots réponse ordi
		_gameSlotCount = 4;
		// envoi signal à LabelEssaiMax => joueur a 6 essais
		// envoi signal à game logic => joueur a 6 essais
		// si joueur réussi 5 fois => les biominds sauvent le monde
		// débloque le mode chrono !
	}
	
	
	//  == GESTION RESET BOARD BUTTONS == //
	
	// Reset de la ligne en jeu
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
	}
	
	public void _on_validate_button_pressed()
	{
		// d'abord on s'assure que toute la ligne est remplie sinon
		// petite animation pas content et return
		var playerSlotContainer = _boardButtonFocused.GetParent().GetParent();
		
		// RAZ playerGuess entre chaque ligne
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
		GD.Print(playerGuess + "dans gamezone avant envoi");
		// Ensuite il y a l'envoi à la logique de jeu GameLogic
		EmitSignal(SignalName.OnPlayerGuessComplete, playerGuess);
		
		
	}
	
	public void ManageHints(int rightPosition, int wrongPosition)
	{
		_currentRow = _boardButtonFocused.FindParent("AttemptRow*") as AttemptRow;
		var hintButtons = _currentRow.FindChildren("HintButton*");
		
		int hintIndex = 0;
		
		
		// Dessiner les hints rouges (bien placés)
		for (int i = 0; i < rightPosition; i++)
		{
			if (hintIndex < hintButtons.Count)
			{
				(hintButtons[hintIndex] as HintButton)?.RedHint();
				hintIndex++;
			}
		}
		
		// Dessiner les hints blancs (mal placés)
		for (int i = 0; i < wrongPosition; i++)
		{
			if (hintIndex < hintButtons.Count)
			{
				(hintButtons[hintIndex] as HintButton)?.WhiteHint();
				hintIndex++;
			}
		}
	}
	
	public void NextRow()
	{
		
		// Ensuite on détermine à quelle ligne on est pour passer à la suivante
		_currentRow = _boardButtonFocused.FindParent("AttemptRow*") as AttemptRow;
		_currentRow.StopAnim();
		
		var parentRow = _currentRow.GetParent();
		int currentIndex = _currentRow.GetIndex();
		
		GD.Print("currentIndex = ", currentIndex);
		GD.Print("parentRow.GetChildCount = ", parentRow.GetChildCount());
		
		// Vérifier qu'il y a bien une ligne suivante et qu'on est pas à la dernière
		if (currentIndex < parentRow.GetChildCount())
		{
			foreach  (AttemptRow child in parentRow.GetChildren())
			{
				// seules les lignes visibles sont concernées
				if (child.Visible == true)
				{
					_nextRow = parentRow.GetChild(currentIndex + 1) as AttemptRow;
					_nextRow.FocusFirstInRow();
				}
			}
		}
	}
	
	// Reset de tout le plateau en cas de "rejouer"
	public void ResetAllBoardButtons()
	{
		// Efface toutes les réponses de la partie précédente
		var clearAll = FindChildren("BoardButton");
		foreach (BoardButton button in clearAll)
		{
			button.ClearIcon();
		}
		// Reset le Focus au 1er bouton
		_on_play_btn_pressed();
	}
	
	// MAJ Compteur "Tentative"
	private void UpdateAttemptLabel(int current, int max)
	{
		LabelNbTentative.Text = current.ToString();
		GD.Print($"Essai {current}/{max}");
	}
	
}	
