using Godot;
using System;

public partial class EndScreen : Control
{
	[Export] Brain Brain;
	[Export] HomeScreen HomeScreen;
	[Export] OrbTexture OrbTexture;
	
	[Export] PackedScene ResultSlotScene;
	[Export] HBoxContainer ResultSlotContainer;
	
	[Export] AnimationPlayer EndScreenAnim;
	[Export] AnimationPlayer TopEndAnim;
	
	[Export] Label ResultLabel;
	[Export] Control WinCases;
	[Export] Control LoseCases;
	[Export] Control Hard;
	[Export] Control EasyOrMedium;
	[Export] VBoxContainer OrbSpawn;
	[Export] VBoxContainer TreasureSpawn;
	
	ConfigFile config = new ConfigFile();
	
	private string _difficultyMode;
	private int _countDown = 5;
	private int _randomOrb = 0;
	private bool _firstBlack = false;
	private bool _firstBlue = false;
	private bool _firstPurple = false;
	private bool _firstRed = false;
	private bool _firstWhite = false;
	private bool _firstYellow = false;
	public int _orbCount;
	private bool _isSixOrbsObtained = false;
	
	
	public override void _Ready()
	{
		GD.Print("_orbCount au ready = " + _orbCount);
		Brain.DisplaySolution += OnDisplaySolution;
		WinCases.Visible = false;
		LoseCases.Visible = false;
		Hard.Visible = false;
		EasyOrMedium.Visible = false;
		OrbSpawn.Visible = true;
		TreasureSpawn.Visible = false;
		
		LoadData();
	}
	
	//===============================================================
	// INSTANTIATION DES EMPLACEMENTS INDICES
	//===============================================================
	public void InstantiateResultSlots(int slots)
	{
		// RAZ préventive
		foreach (Node child in ResultSlotContainer.GetChildren())
		{
			ResultSlotContainer.RemoveChild(child);
			child.QueueFree();
		}
		
		// Instantiation des Emplacements de résultat pour l'écran final (ResultSlot)
		for (int i = 0; i < slots; i++)
		{
			var slotInstance = ResultSlotScene.Instantiate();
			slotInstance.Name = $"ResultSlot_{i}";		// <= forcer nommage en ResultSlot_1
			
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
			case 1:  _difficultyMode = "Facile"; break;
			case 2:  _difficultyMode = "Moyen"; break;
			case 3:  _difficultyMode = "Difficile"; break;
			case 4:  _difficultyMode = "Expert"; break;
		}
		GD.Print("_difficultyMode dans EndScreen = " + _difficultyMode);
	}
	
	//===============================================================
	// 	RANDOMISATION DE LA RECOMPENSE (ORBE) EN HARD (DIFFICILE)
	//===============================================================
	private void OrbRandomSelector()
	{
		_randomOrb = (int)(GD.Randi() % 6);
		
			switch (_randomOrb)
			{
				case 0: OrbTexture.Select((int)OrbTexture.OrbColor.black); break;
				case 1: OrbTexture.Select((int)OrbTexture.OrbColor.blue);  break;
				case 2: OrbTexture.Select((int)OrbTexture.OrbColor.purple); break;
				case 3: OrbTexture.Select((int)OrbTexture.OrbColor.red); break;
				case 4: OrbTexture.Select((int)OrbTexture.OrbColor.white); break;
				case 5: OrbTexture.Select((int)OrbTexture.OrbColor.yellow); break;
			}
		GD.Print("randomOrb = " + _randomOrb);

	}
	
	//===============================================================
	// ENVOI DE LA RECOMPENSE DANS LA COLLECTION (HOMESCREEN)
	//===============================================================
	private void GetOrbInCollec()
	{
		GD.Print("_orbCount dans GetOrbInCollec AVANT ajout de l'obe gagnée = " + _orbCount);
		if (_randomOrb == 0 && _firstBlack == false)
		{
			HomeScreen.DisplayOrb(_randomOrb);
			_firstBlack = true;
			_orbCount++;
			config.SetValue("Player", "BlackOrbObtained", true);
			config.Save("user://endscreen.cfg");
		}
		if (_randomOrb == 1 && _firstBlue == false)
		{
			HomeScreen.DisplayOrb(_randomOrb);
			_firstBlue = true;
			_orbCount++;
			config.SetValue("Player", "BlueOrbObtained", true);
			config.Save("user://endscreen.cfg");
		}
		if (_randomOrb == 2 && _firstPurple == false)
		{
			HomeScreen.DisplayOrb(_randomOrb);
			 _firstPurple = true;
			_orbCount++;
			config.SetValue("Player", "PurpleOrbObtained", true);
			config.Save("user://endscreen.cfg");
		}
		if (_randomOrb == 3 && _firstRed == false)
		{
			HomeScreen.DisplayOrb(_randomOrb);
			_firstRed = true;
			_orbCount++;
			config.SetValue("Player", "RedOrbObtained", true);
			config.Save("user://endscreen.cfg");
		}
		if (_randomOrb == 4 && _firstWhite == false)
		{
			HomeScreen.DisplayOrb(_randomOrb);
			 _firstWhite = true;
			_orbCount++;
			config.SetValue("Player", "WhiteOrbObtained", true);
			config.Save("user://endscreen.cfg");
		}
		if (_randomOrb == 5 &&  _firstYellow == false)
		{
			HomeScreen.DisplayOrb(_randomOrb);
			 _firstYellow = true;
			_orbCount++;
			config.SetValue("Player", "YellowOrbObtained", true);
			config.Save("user://endscreen.cfg");
		}
		GD.Print("_orbCount dans GetOrbInCollec APRES ajout de l'obe gagnée = " + _orbCount);
	}
	
	//===============================================================
	// AFFICHAGE ECRAN FIN SUIVANT WIN OU LOSE // ET DIFFICULTE
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
				if (_orbCount <= 6)
				{
					Hard.Visible = true;
					OrbRandomSelector();
					TopEndAnim.Play("RESET");
					TopEndAnim.Play("WinHard"); 
					HomeScreen.DisplayOrbCollection();
					GetOrbInCollec();
					GD.Print("_orbCount dans WinCase AVANT if (_orbCount == 6) = " + _orbCount);
					
					if (_orbCount == 6 && _isSixOrbsObtained == false)
					{
						_isSixOrbsObtained = true;
						OnSixOrbsObtained();
						config.SetValue("Player", "IsSixOrbsObtained", true);
						config.Save("user://endscreen.cfg");
					}
				}
				
				if (_orbCount >= 6 && _isSixOrbsObtained == true)
				{
					Hard.Visible = false;
					EasyOrMedium.Visible = true;
					TopEndAnim.Play("RESET");
					TopEndAnim.Play("WinEasyMed");
				}
				break;
				
			case "Expert": 
				EasyOrMedium.Visible = true;
				TopEndAnim.Play("RESET");
				TopEndAnim.Play("WinExpert"); 
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
	
	private async void OnSixOrbsObtained()
	{
		await ToSignal(TopEndAnim, "animation_finished");
		
		TreasureSpawn.Visible = true;
		OrbSpawn.Visible = false;
		
		TopEndAnim.Play("RESET");
		TopEndAnim.Play("SixOrbs");
		
		HomeScreen.OnSixOrbsCompleted();
		
		GD.Print("Le joueur a obtenu tous les Orbes !");
		GD.Print("Affiche le coffre ouvert + bravo vous avez obtenu tous les Orbes ");
		GD.Print("Un nouveau mode de difficulté a été débloqué ! -> paf on débloque le mode Expert et voilà");
		GD.Print("Ensuite on réplique le code pour le Expert mode  -> pour débloquer autre chose ");
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
	
	private void LoadData()
	{
		Error err = config.Load("user://endscreen.cfg");
		
		bool BlackOrbObtained = false; 
		bool BlueOrbObtained = false; 
		bool PurpleOrbObtained = false; 
		bool RedOrbObtained = false; 
		bool WhiteOrbObtained = false; 
		bool YellowOrbObtained = false; 
		
		bool IsSixOrbsObtained = false;
		
		BlackOrbObtained = (bool)config.GetValue("Player", "BlackOrbObtained", false);
		BlueOrbObtained = (bool)config.GetValue("Player", "BlueOrbObtained", false);
		PurpleOrbObtained = (bool)config.GetValue("Player", "PurpleOrbObtained", false);
		RedOrbObtained = (bool)config.GetValue("Player", "RedOrbObtained", false);
		WhiteOrbObtained = (bool)config.GetValue("Player", "WhiteOrbObtained", false);
		YellowOrbObtained = (bool)config.GetValue("Player", "YellowOrbObtained", false);
		
		IsSixOrbsObtained = (bool)config.GetValue("Player", "IsSixOrbsObtained", false);
		
		_firstBlack = BlackOrbObtained;
		_firstBlue = BlueOrbObtained;
		_firstPurple = PurpleOrbObtained;
		_firstRed = RedOrbObtained;
		_firstWhite = WhiteOrbObtained;
		_firstYellow = YellowOrbObtained;
		
		_isSixOrbsObtained = IsSixOrbsObtained;
		
		_orbCount = 0;
		if (_firstBlack) _orbCount++;
		if (_firstBlue) _orbCount++;
		if (_firstPurple) _orbCount++;
		if (_firstRed) _orbCount++;
		if (_firstWhite) _orbCount++;
		if (_firstYellow) _orbCount++;
		
		GD.Print("_orbCount à la fin de LoadData() = " + _orbCount);
	}

}
