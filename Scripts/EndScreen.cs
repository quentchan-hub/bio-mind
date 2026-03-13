using Godot;
using System;

public partial class EndScreen : Control
{
	[Export] Brain Brain;
	[Export] HomeScreen HomeScreen;

	[Export] PackedScene ResultSlotScene;
	[Export] HBoxContainer ResultSlotContainer;

	[Export] AnimationPlayer EndScreenAnim;
	[Export] AnimationPlayer TopEndAnim;

	[Export] Label ResultLabel;

	// - Animations en cas de GAGNE (WinCases) -
	[Export] Control WinCases;

	//	-> Cas 1 : en Facile ou Moyen
	[Export] Control EasyOrMedium;

	//	-> Cas 2 : en Difficile
	[Export] Control Hard;
	[Export] OrbSpawn OrbSpawn;
	[Export] VBoxContainer TreasureSpawn;

	//	-> Cas 3 : en Expert
	[Export] Control Expert;
	[Export] RobotPartSpawn RobotPartSpawn;

	// - Animations en cas de PERTE (LoseCases) -
	[Export] Control LoseCases;

	// - Variables Internes
	private string _difficultyMode;
	public ConfigFile config = new ConfigFile();

	private int _randomOrb = 0;
	private int _randomPart = 0;

	public bool _isSixOrbsObtained = false;
	public bool _isEightPartsObtained = false;

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
					Hard.Visible = true;
					OrbRandomSelector();
					TopEndAnim.Play("RESET");
					TopEndAnim.Play("WinHard");
					OrbSpawn.GetOrbInCollec(_randomOrb);
					GD.Print("orbCount dans WinCase = " + OrbSpawn.orbCount);

					if (OrbSpawn.orbCount == 6 && _isSixOrbsObtained == false)
					{
						_isSixOrbsObtained = true;
						config.SetValue("Player", "IsSixOrbsObtained", true);
						config.Save("user://endscreen.cfg");
						OnSixOrbsObtained();
					}
				}
				else if (_isSixOrbsObtained == true)
				{
					EasyOrMedium.Visible = true;
					TopEndAnim.Play("RESET");
					TopEndAnim.Play("WinEasyMed");
				}
				break;

			case "Expert":
				if (RobotPartSpawn.partCount < 8)
				{
					Expert.Visible = true;
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

}
