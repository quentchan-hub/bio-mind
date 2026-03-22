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

	[Export] Label ResultLabel;

	// - Animations en cas de GAGNE (WinCases) -
	[Export] Control WinCases;

	//	-> Cas 1 : en Facile ou Moyen
	[Export] Control EasyOrMedium;

	//	-> Cas 2 : en Difficile
	[Export] Control Hard;
	[Export] OrbSpawn OrbSpawn;
	[Export] VBoxContainer TreasureSpawn;
	[Export] Control OrbSpawnFilter;

	//	-> Cas 3 : en Expert
	[Export] Control Expert;
	[Export] RobotPartSpawn RobotPartSpawn;
	[Export] VBoxContainer RobotCompleteSpawn;

	// - Animations en cas de PERTE (LoseCases) -
	[Export] Control LoseCases;

	// - Variables Internes
	private string _difficultyMode;
	public ConfigFile config = new ConfigFile();

	private int _randomOrb = 0;
	private int _randomPart = 0;

	public bool _isSixOrbsObtained = false;
	public bool _isEightPartsObtained = false;
	private bool _isNewOrbColor = false;

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

		LoadData();
	}

	//===============================================================
	// INSTANTIATION DES EMPLACEMENTS INDICES
	//===============================================================
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

	//===============================================================
	// MAJ ECRAN DE FIN SUIVANT DIFFICULTE
	//===============================================================
	public void DifficultyMode(int gameDifficulty)
	{
		switch (gameDifficulty)
		{
			case 1: _difficultyMode = "Facile"; break;
			case 2: _difficultyMode = "Moyen"; break;
			case 3: _difficultyMode = "Difficile"; break;
			case 4: _difficultyMode = "Expert"; break;
		}
		GD.Print("_difficultyMode dans EndScreen = " + _difficultyMode);
	}

	//===============================================================
	// RANDOMISATION RECOMPENSE ORBE (DIFFICILE)
	//===============================================================
	private void OrbRandomSelector()
	{
		_randomOrb = (int)(GD.Randi() % 6);
		OrbSpawn.SelectOrb(_randomOrb);
		GD.Print("randomOrb = " + _randomOrb);
	}

	//===============================================================
	// RANDOMISATION RECOMPENSE PART (EXPERT)
	//===============================================================
	private void PartRandomSelector()
	{
		_randomPart = (int)(GD.Randi() % 8);
		RobotPartSpawn.SelectPart(_randomPart);
		GD.Print("_randomPart = " + _randomPart);
	}

	//===============================================================
	// AFFICHAGE ECRAN FIN SUIVANT WIN OU LOSE
	//===============================================================
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
				EasyOrMedium.Visible = true;
				TopEndAnim.Play("RESET");
				TopEndAnim.Play("WinEasyMed");
				break;

			case "Moyen":
				EasyOrMedium.Visible = true;
				TopEndAnim.Play("RESET");
				TopEndAnim.Play("WinEasyMed");
				break;
				
			case "Difficile":
				
				if (OrbSpawn.orbCount < 6)
				{
					PlayHardRewardSequence();
				}
				if (OrbSpawn.orbCount == 6 && _isSixOrbsObtained == false)
				{
					_isSixOrbsObtained = true;
					config.SetValue("Player", "IsSixOrbsObtained", true);
					config.Save("user://endscreen.cfg");
					OnSixOrbsObtained();
				}
				else if (OrbSpawn.orbCount >= 6 && _isSixOrbsObtained == true)
				{
					GD.Print("test le cas du 6 orbs = déjà true");
					EasyOrMedium.Visible = true;
					TopEndAnim.Play("RESET");
					TopEndAnim.Play("WinEasyMed");
				}
				break;

			case "Expert":
				if (RobotPartSpawn.partCount < 8)
				{
					Expert.Visible = true;
					RobotPartSpawn.Visible = true;
					RobotCompleteSpawn.Visible = false;
					PartRandomSelector();
					TopEndAnim.Play("RESET");
					TopEndAnim.Play("WinExpert");
					RobotPartSpawn.GetPartInCollec(_randomPart);
					GD.Print("partCount dans WinCase = " + RobotPartSpawn.partCount);

					if (RobotPartSpawn.partCount == 8 && _isEightPartsObtained == false)
					{
						_isEightPartsObtained = true;
						config.SetValue("Player", "IsEightPartsObtained", true);
						config.Save("user://endscreen.cfg");
						OnEightPartsObtained();
					}
				}
				else if (_isEightPartsObtained == true)
				{
					EasyOrMedium.Visible = true;
					TopEndAnim.Play("RESET");
					TopEndAnim.Play("WinEasyMed");
				}
				break;
		}
	}
	
	private async void PlayHardRewardSequence()
	{
		Hard.Visible = true;
		OrbSpawn.Visible = true;
		TreasureSpawn.Visible = false;

		OrbRandomSelector();

		TopEndAnim.Play("RESET");
		TopEndAnim.Play("WinHard");
		await ToSignal(TopEndAnim, "animation_finished");
		
		// si l'orbe tirée est d'une nouvelle couleur 
		_isNewOrbColor = OrbSpawn.GetOrbInCollec(_randomOrb); 
		GD.Print("isNewOrbColor");
		if (_isNewOrbColor) {TopEndAnim.Play("NewOrbColor");}
		else {TopEndAnim.Play("OldOrbColor");}
		
		EndOrbs.DisplayEndOrbCollection();
		EndOrbs.DisplayEndOrb(_randomOrb);

		GD.Print("orbCount dans WinCase = " + OrbSpawn.orbCount);
	}
	
	private async void _on_orb_spawn_button_pressed()
	{
		
		TopEndAnim.Play("CloseAnnouncePanel");
		if (_isNewOrbColor)
		{
			RayOrbAnim.Play(GetRayAnimName(_randomOrb));
			await ToSignal(RayOrbAnim, "animation_finished");
		}
		OrbSpawnFilter.Visible = false;
	}
	
	private string GetRayAnimName(int orb)
	{
		return orb switch
		{
			0 => "RayOrbBlack0",
			1 => "RayOrbBlue1",
			2 => "RayOrbPurple2",
			3 => "RayOrbRed3",
			4 => "RayOrbWhite4",
			5 => "RayOrbYellow5",
			_ => "RayOrbBlack0"
		};
	}
	
	public void LoseCase()
	{
		WinCases.Visible = false;
		LoseCases.Visible = true;
		ResultLabel.Text = "PERDU !";
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("LosePose");
	}

	//===============================================================
	// SEQUENCE 6 ORBES OBTENUES
	//===============================================================
	private async void OnSixOrbsObtained()
	{
		await ToSignal(TopEndAnim, "animation_finished");
		TreasureSpawn.Visible = true;
		OrbSpawn.Visible = false;
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("SixOrbs");
		HomeScreen.OnSixOrbsCompleted();
		GD.Print("Le joueur a obtenu tous les Orbes !");
	}

	//===============================================================
	// SEQUENCE 8 PARTS OBTENUES
	//===============================================================
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

	//===============================================================
	// AFFICHAGE DE LA SOLUTION (CODE)
	//===============================================================
	private void OnDisplaySolution(Godot.Collections.Array<int> solution)
	{
		EndScreenAnim.Play("MidGlowEffect");
		var slots = ResultSlotContainer.GetChildren();
		for (int i = 0; i < solution.Count; i++)
		{
			var slot = slots[i];
			ResultButton button = slot.GetChild(0) as ResultButton;
			button.SetResult(solution[i]);
		}
	}

	//===============================================================
	// SAUVEGARDE / CHARGEMENT
	//===============================================================
	private void LoadData()
	{
		Error err = config.Load("user://endscreen.cfg");
		if (err != Error.Ok) return;
		_isSixOrbsObtained =   (bool)config.GetValue("Player", "IsSixOrbsObtained", false);
		_isEightPartsObtained = (bool)config.GetValue("Player", "IsEightPartsObtained", false);
		GD.Print("_isSixOrbsObtained chargé = " + _isSixOrbsObtained);
		GD.Print("_isEightPartsObtained chargé = " + _isEightPartsObtained);
	}
	
	public void ResetData()
	{
		config = new ConfigFile();
		_Ready();
	}


}
