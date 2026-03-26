using Godot;
using System;

public partial class EndScreen : Control
{
	[Export] Brain Brain;
	[Export] HomeScreen HomeScreen;
	[Export] EndOrbs EndOrbs;

	[Export] PackedScene ResultSlotScene;
	[Export] HBoxContainer ResultSlotContainer;
	[Export] MarginContainer OrbSpawnText;

	[Export] AnimationPlayer EndScreenAnim;
	[Export] AnimationPlayer TopEndAnim;
	[Export] AnimationPlayer RayOrbAnim;
	[Export] AnimationPlayer AnnounceAnim;

	[Export] Label ResultLabel;

	[Export] Control WinCases;
	
	[Export] Control EasyOrMedium;
	[Export] ThumbsUpPose ThumbsUpPose;
	
	[Export] Control Hard;
	[Export] OrbSpawn OrbSpawn;
	[Export] VBoxContainer TreasureSpawn;

	[Export] Control Expert;
	[Export] RobotPartSpawn RobotPartSpawn;
	[Export] VBoxContainer RobotCompleteSpawn;

	[Export] Control LoseCases;
	[Export] Control ButtonBlocker;

	private string _difficultyMode;
	public ConfigFile config = new ConfigFile();
	
	private int _randomOrb = 0;
	private bool _isNewOrbColor = false;
	public bool _isSixOrbsObtained = false;
	
	private int _randomPart = 0;
	private bool _isNewPart = false;
	public bool _isEightPartsObtained = false;
	
	// Variables Test/debug
	[Export] public bool DevForceOrbEnabled = false;
	[Export] public int DevForceOrbIndex = 0;

	public override void _Ready()
	{
		Brain.DisplaySolution += OnDisplaySolution;

		WinCases.Visible = false;
		EasyOrMedium.Visible = false;
		Hard.Visible = false;
		Expert.Visible = false;
		OrbSpawn.Visible = true;
		TreasureSpawn.Visible = false;
		LoseCases.Visible = false;
		ButtonBlocker.Visible = false;

		LoadData();
	}

	// ================================================================
	// GAME / HINT SETUP
	// ================================================================

	public void InstantiateResultSlots(int slots)
	{
		foreach (Node child in ResultSlotContainer.GetChildren())
		{
			ResultSlotContainer.RemoveChild(child);
			child.QueueFree();
		}
		for (int i = 0; i < slots; i++)
		{
			var slotInstance = ResultSlotScene.Instantiate();
			slotInstance.Name = $"ResultSlot_{i}";
			ResultSlotContainer.AddChild(slotInstance);
		}
	}

	public void DifficultyMode(int gameDifficulty)
	{
		_difficultyMode = gameDifficulty switch
		{
			1 => "Facile",
			2 => "Moyen",
			3 => "Difficile",
			4 => "Expert",
			_ => "Facile"
		};
		GD.Print("Niveau = " + _difficultyMode);
	}

	// ================================================================
	// WIN / LOSE CASES
	// ================================================================

	public void WinCase()
	{
		WinCases.Visible = true;
		LoseCases.Visible = false;
		Hard.Visible = false;
		EasyOrMedium.Visible = false;
		ResultLabel.Text = "BRAVO !";

		switch (_difficultyMode)
		{
			case "Facile":
			case "Moyen":
				PlayEasyMedWin();
				break;

			case "Difficile":
				OnHardWinCase();
				break;

			case "Expert":
				OnExpertWinCase();
				break;
		}
	}

	public void LoseCase()
	{
		WinCases.Visible = false;
		LoseCases.Visible = true;
		ResultLabel.Text = "PERDU !";
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("LosePose");
	}

	// ================================================================
	// DIFFICULTY WIN SEQUENCES
	// ================================================================

	private void PlayEasyMedWin()
	{
		EasyOrMedium.Visible = true;
		ThumbsUpPose.ThumbsUpRandomSelector();
		AnnounceAnim.Play("RESET");
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("WinEasyMed");
	}

	// Toute la séquence Difficile dans une seule méthode async.
	// ButtonBlocker s'allume au début et s'éteint dans tous les cas de sortie.
	private async void OnHardWinCase()
	{
		ButtonBlocker.Visible = true;

		// Cas : toutes les orbes déjà obtenues, rien à animer
		if (OrbSpawn.orbCount >= 6 && _isSixOrbsObtained)
		{
			GD.Print("6 orbes déjà obtenues, on passe direct.");
			PlayEasyMedWin();
			ButtonBlocker.Visible = false;
			return;
		}

		// Cas : récompense à jouer (orbCount < 6, ou 6e orbe pas encore comptée)
		await PlayHardRewardSequence();

		// Mise à jour état si c'est la 6e
		if (OrbSpawn.orbCount == 6 && !_isSixOrbsObtained)
		{
			_isSixOrbsObtained = true;
			config.SetValue("Player", "IsSixOrbsObtained", true);
			config.Save("user://endscreen.cfg");
		}

		// Le ButtonBlocker se désactive à la fin du bouton Valider (voir _on_orb_spawn_button_pressed)
		// On ne le désactive pas ici car l'animation RayOrb + SixOrbs n'est pas encore terminée
	}

	private void OnExpertWinCase()
	{
		ButtonBlocker.Visible = true;
		
		if (_isEightPartsObtained)
		{
			PlayEasyMedWin(); // à modifier en séquence combat Super Robot contre super mechant
			ButtonBlocker.Visible = false;
			return;
		}
		
		Expert.Visible = true;
		RobotPartSpawn.Visible = true;
		RobotCompleteSpawn.Visible = false;
		
		if (RobotPartSpawn.partCount < 8)
		{
			PartRandomSelector();
			TopEndAnim.Play("RESET");
			TopEndAnim.Play("WinExpert");
			
			// récupère l'info si nouvelle relique (_isNewPart) ou pas
			_isNewPart = RobotPartSpawn.GetPartInCollec(_randomPart);
			GD.Print("_isNewPart = " + _isNewPart);
			GD.Print("partCount = " + RobotPartSpawn.partCount);
			
			// si nouvelle relique -> joue anim NewPart, sinon -> anim OldPart
			AnnounceAnim.Play(_isNewPart ? "NewPart" : "OldPart");
		}

		
		// si les 8 parties on été trouvées 
		if (RobotPartSpawn.partCount == 8)
		{
			_isEightPartsObtained = true;
			AnnounceAnim.Play("EightParts");
			config.SetValue("Player", "IsEightPartsObtained", true);
			config.Save("user://endscreen.cfg");
			OnEightPartsObtained();
			return;
		}
		
	}
	
	private void _on_robot_parts_button_pressed()
	{
		AnnounceAnim.Play("CloseAnnouncePanel");
		ButtonBlocker.Visible = false;
	}
		
	// ================================================================
	// HARD REWARD SEQUENCE (appelée depuis OnHardWinCase)
	// ================================================================

	private async System.Threading.Tasks.Task PlayHardRewardSequence()
	{
		Hard.Visible = true;
		OrbSpawn.Visible = true;
		TreasureSpawn.Visible = false;

		//selectionne aléatoirement l'orbe gagnée (Dans OrbSpawn)
		OrbRandomSelector();

		//joue l'animation de remise de l'orbe gagnée
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("WinHard");
		await ToSignal(TopEndAnim, "animation_finished");

		// récupère l'info si nouvelle couleur d'orbe (_isNewOrbColor) ou pas
		_isNewOrbColor = OrbSpawn.GetOrbInCollec(_randomOrb);
		GD.Print("isNewOrbColor = " + _isNewOrbColor);

		// si nouvelle couleur -> joue anim NewOrbColor, sinon -> anim OldOrbColor
		TopEndAnim.Play(_isNewOrbColor ? "NewOrbColor" : "OldOrbColor");

		EndOrbs.DisplayEndOrbCollection();
	}

	// Déclenché par le bouton Valider sur l'écran d'annonce
	private async void _on_orb_spawn_button_pressed()
	{
		TopEndAnim.Play("CloseAnnouncePanel");

		if (_isNewOrbColor)
		{
			RayOrbAnim.Play(GetRayAnimName(_randomOrb));
			await ToSignal(RayOrbAnim, "animation_finished");

			EndOrbs.DisplayEndOrb(_randomOrb);

			if (OrbSpawn.orbCount == 6)
			{
				await OnSixOrbsObtained();
				return;
			}
		}
		HideButtonBlocker();
	}
	
	private void _on_six_orb_button_pressed()
	{
		TopEndAnim.Play("CloseAnnouncePanel");
		HomeScreen.ExpertModeOpen();
		HideButtonBlocker();
	}
	private void HideButtonBlocker()
	{
		ButtonBlocker.Visible = false;
	}
	// ================================================================
	// COLLECTIONS
	// ================================================================

	private async System.Threading.Tasks.Task OnSixOrbsObtained()
	{
		OrbSpawn.Visible = false;
		TreasureSpawn.Visible = true;
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("SixOrbs");
		HomeScreen.OnSixOrbsCompleted();
		await ToSignal(TopEndAnim, "animation_finished");
		
		GD.Print("Le joueur a obtenu tous les Orbes !");
	}

	private async void OnEightPartsObtained()
	{
		await ToSignal(TopEndAnim, "animation_finished");
		RobotPartSpawn.Visible = false;
		RobotCompleteSpawn.Visible = true;
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("EightParts");
		HomeScreen.OnEightPartsCompleted();
		GD.Print("Le joueur a obtenu toutes les parties du robot !");
	}

	// ================================================================
	// HELPERS
	// ================================================================

	private void OrbRandomSelector()
	{
		_randomOrb = DevForceOrbEnabled
			? Mathf.Clamp(DevForceOrbIndex, 0, 5)
			: (int)(GD.Randi() % 6);
		OrbSpawn.SelectOrb(_randomOrb);
		GD.Print("randomOrb = " + _randomOrb);
	}

	private void PartRandomSelector()
	{
		_randomPart = (int)(GD.Randi() % 8);
		RobotPartSpawn.SelectPart(_randomPart);
		GD.Print("_randomPart = " + _randomPart);
	}

	private static string GetRayAnimName(int orb) => orb switch
	{
		0 => "RayOrbBlack0",
		1 => "RayOrbBlue1",
		2 => "RayOrbPurple2",
		3 => "RayOrbRed3",
		4 => "RayOrbWhite4",
		5 => "RayOrbYellow5",
		_ => "RayOrbBlack0"
	};

	// ================================================================
	// SOLUTION DISPLAY
	// ================================================================

	private void OnDisplaySolution(Godot.Collections.Array<int> solution)
	{
		EndScreenAnim.Play("MidGlowEffect");
		var slots = ResultSlotContainer.GetChildren();
		for (int i = 0; i < solution.Count; i++)
		{
			ResultButton button = slots[i].GetChild(0) as ResultButton;
			button.SetResult(solution[i]);
		}
	}

	// ================================================================
	// SAVE / LOAD / RESET
	// ================================================================

	private void LoadData()
	{
		Error err = config.Load("user://endscreen.cfg");
		if (err != Error.Ok) return;
		_isSixOrbsObtained =    (bool)config.GetValue("Player", "IsSixOrbsObtained", false);
		_isEightPartsObtained = (bool)config.GetValue("Player", "IsEightPartsObtained", false);
	}

	public void ResetData()
	{
		config = new ConfigFile();
		_Ready();
	}

	public void ResetEndScreenAndRewards()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("endscreen.cfg");
		dir.Remove("orbspawn.cfg");
		dir.Remove("robotpartspawn.cfg");

		ResetData();
		OrbSpawn.ResetData();
		RobotPartSpawn.ResetData();
		EndOrbs.ResetData();

		OrbSpawn.orbCount = 0;
		RobotPartSpawn.partCount = 0;
	}
	
}
