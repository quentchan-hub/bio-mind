using Godot;
using System;

public partial class HomeScreen : Control
{
	[Signal] public delegate void StartGameEventHandler();
	
	[Export] Brain Brain;
	[Export] EndScreen EndScreen;
	
	[Export] Control OptionsOverlay;
	[Export] ConfirmationDialog ConfirmReset;
	
	[Export] PanelContainer DisplayCollectInstruct;
	[Export] MarginContainer OrbCollection;

	[Export] TextureRect OrbBlack;
	[Export] TextureRect OrbBlue;
	[Export] TextureRect OrbPurple;
	[Export] TextureRect OrbRed;
	[Export] TextureRect OrbWhite;
	[Export] TextureRect OrbYellow;
	
	[Export] Button ExpertBtn;
	
	ConfigFile configFile = new ConfigFile();
	
	// ready
	public override void _Ready()
	{
		OptionsOverlay.Visible = false;
		ConfirmReset.Visible = false;
		
		OrbBlack.Visible = false;
		OrbBlue.Visible = false; 
		OrbPurple.Visible = false; 
		OrbRed.Visible = false;
		OrbWhite.Visible = false;  
		OrbYellow.Visible = false; 
		
		OrbCollection.Visible = false;
		DisplayCollectInstruct.Visible = true;
		
		ExpertBtn.Visible = false; 
		
		LoadData();
	}

	// process
	public override void _Process(double delta)
	{
	}
	
	private void _on_options_btn_pressed()
	{
		OptionsOverlay.Visible = true;
	}
	
	private void _on_close_options_button_pressed()
	{
		OptionsOverlay.Visible = false;
	}
	
	public void DisplayOrbCollection()
	{
		OrbCollection.Visible = true;
		DisplayCollectInstruct.Visible = false;
		SaveData();
	}
	
	private void SaveData()
	{
		configFile.SetValue("UI", "OrbCollectionUnlocked", true);
		configFile.Save("user://homescreen.cfg");
	}
	
	private void LoadData()
	{
		//var configFile = new ConfigFile();
		Error err = configFile.Load("user://homescreen.cfg");
		
		bool orbUnlocked = false;
		
		bool blackUnlocked = false;
		bool blueUnlocked = false;
		bool purpleUnlocked = false;
		bool redUnlocked = false;
		bool whiteUnlocked = false;
		bool yellowUnlocked = false;
		bool expertVisible = false;

		if (err == Error.Ok)
		{
			orbUnlocked = (bool)configFile.GetValue("UI", "OrbCollectionUnlocked", false);
			
			blackUnlocked = (bool)configFile.GetValue("Player", "OrbBlackUnlocked", false);
			blueUnlocked = (bool)configFile.GetValue("Player", "OrbBlueUnlocked", false);
			purpleUnlocked = (bool)configFile.GetValue("Player", "OrbPurpleUnlocked", false);
			redUnlocked = (bool)configFile.GetValue("Player", "OrbRedUnlocked", false);
			whiteUnlocked = (bool)configFile.GetValue("Player", "OrbWhiteUnlocked", false);
			yellowUnlocked = (bool)configFile.GetValue("Player", "OrbYellowUnlocked", false);
			
			expertVisible = (bool)configFile.GetValue("UI", "ExpertButtonVisible", false);
			
			OrbCollection.Visible = orbUnlocked;
			DisplayCollectInstruct.Visible = !orbUnlocked;
			OrbBlack.Visible = blackUnlocked;
			OrbBlue.Visible = blueUnlocked;
			OrbPurple.Visible = purpleUnlocked;
			OrbRed.Visible = redUnlocked;
			OrbWhite.Visible = whiteUnlocked;
			OrbYellow.Visible = yellowUnlocked;
			ExpertBtn.Visible = expertVisible;
		}
	}
	
	public void DisplayOrb(int orbEarned)
	{
		GD.Print("dans HomeScreen orbe gagnée (0 - 5) = " + orbEarned);
		switch(orbEarned)
		{
			case 0: 
				OrbBlack.Visible = true; 
				configFile.SetValue("Player", "OrbBlackUnlocked", true);
				configFile.Save("user://homescreen.cfg");
				break; 

				
			case 1: 
				OrbBlue.Visible = true; 
				configFile.SetValue("Player", "OrbBlueUnlocked", true);
				configFile.Save("user://homescreen.cfg");
				break;
				
			case 2: 
				OrbPurple.Visible = true; 
				configFile.SetValue("Player", "OrbPurpleUnlocked", true);
				configFile.Save("user://homescreen.cfg");
				break;
				
			case 3: 
				OrbRed.Visible = true; 
				configFile.SetValue("Player", "OrbRedUnlocked", true);
				configFile.Save("user://homescreen.cfg");
				break;
			
			case 4: 
				OrbWhite.Visible = true; 
				configFile.SetValue("Player", "OrbWhiteUnlocked", true);
				configFile.Save("user://homescreen.cfg");
				break;
				
			case 5:
				OrbYellow.Visible = true; 
				configFile.SetValue("Player", "OrbYellowUnlocked", true);
				configFile.Save("user://homescreen.cfg");
				break;
		}
	}

	private void _on_reset_all_button_pressed()
	{
		ConfirmReset.Visible = true;
	}
	
	private void _on_confirmation_dialog_confirmed()
	{
		DirAccess dir = DirAccess.Open("user://");	// On ouvre le dossier user://
		dir.Remove("homescreen.cfg");		// On supprime le fichier de sauvegarde
		dir.Remove("endscreen.cfg");
		if (FileAccess.FileExists("user://homescreen.cfg"))
			GD.Print("homescreen.cfg existe");
		else {GD.Print("homescreen.cfg n'existe pas");}
	
		if (FileAccess.FileExists("user://endscreen.cfg"))
			GD.Print("endscreen.cfg existe");
		else {GD.Print("endscreen.cfg n'existe pas");}
		
		configFile = new ConfigFile(); // <= vide le config en mémoire !
		
		ResetOrbCollection();
		GD.Print("_orbCount à la reinit = " + EndScreen._orbCount);
	}
	
	public void OnSixOrbsCompleted()
	{
		ExpertBtn.Visible = true; 
		configFile.SetValue("UI", "ExpertButtonVisible", true);
		configFile.Save("user://homescreen.cfg");
		
	}
	
	private void ResetOrbCollection()
	{
		_Ready(); 
		// ferme la fenetre de confirmation + la fenetre d'option
		// RAZ de l'affichage de la collection d'orbes
		// remet les variables de sauvegarde par défaut (false)
	}
}
