using Godot;
using System;
public partial class HomeScreen : Control
{
	[Signal] public delegate void StartGameEventHandler();

	[Export] Brain Brain;
	[Export] EndScreen EndScreen;
	[Export] BotTabContainer BotTabContainer;
	[Export] Control OptionsOverlay;
	[Export] ConfirmationDialog ConfirmReset;
	[Export] Orbs Orbs;
	[Export] Parts Parts;
	[Export] Button ExpertBtn;
	[Export] OrbSpawn OrbSpawn;
	[Export] RobotPartSpawn RobotPartSpawn;

	ConfigFile configFile = new ConfigFile();

	public override void _Ready()
	{
		OptionsOverlay.Visible = false;
		ConfirmReset.Visible = false;
		ExpertBtn.Visible = false;
		LoadData();
	}

	// ===========================
	//     MENU OPTIONS
	// ===========================
	private void _on_options_btn_pressed() => OptionsOverlay.Visible = true;
	private void _on_close_options_button_pressed() => OptionsOverlay.Visible = false;
	private void _on_reset_all_button_pressed() => ConfirmReset.Visible = true;

	private void _on_confirmation_dialog_confirmed()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("homescreen.cfg");
		dir.Remove("endscreen.cfg");
		dir.Remove("orbs.cfg");
		dir.Remove("parts.cfg");
		dir.Remove("orbspawn.cfg");
		dir.Remove("robotpartspawn.cfg");

		configFile = new ConfigFile();
		OrbSpawn.ResetData();
		Orbs.ResetData();
		Parts.ResetData();
		RobotPartSpawn.ResetData();
		EndScreen.ResetData();
		
		BotTabContainer.HidePartsTab();
		BotTabContainer.HideSuperMindTab();
		OrbSpawn.orbCount = 0;
		RobotPartSpawn.partCount = 0;

		ExpertBtn.Visible = false;
		OptionsOverlay.Visible = false;
		ConfirmReset.Visible = false;
		
		GD.Print(" après reset orbCount = " + OrbSpawn.orbCount);
		GD.Print(" après reset partCount = " + RobotPartSpawn.partCount);
	}

	// ============================
	//      ORB COLLECTION
	// ============================
	// -- Collecting --
	public void DisplayOrbCollection() => Orbs.DisplayOrbCollection();
	public void DisplayOrb(int orbEarned) => Orbs.DisplayOrb(orbEarned);

	// -- Orb Collection complete --
	public void OnSixOrbsCompleted()
	{
		ExpertBtn.Visible = true;
		configFile.SetValue("UI", "ExpertButtonVisible", true);
		configFile.Save("user://homescreen.cfg");
		BotTabContainer.ShowPartsTab();
	}

	// ============================
	//    ROBOT PARTS COLLECTION
	// ============================
	// -- Collecting --
	public void DisplayPartCollection() => Parts.DisplayPartCollection();
	public void DisplayPart(int partEarned) => Parts.DisplayPart(partEarned);
	
	// -- Part Collection complete --
	public void OnEightPartsCompleted() 
	{
		configFile.SetValue("UI", "SuperMindTabVisible", true);
		configFile.Save("user://homescreen.cfg");
		BotTabContainer.ShowSuperMindTab();
	} 

	// =============================================
	//   GESTION SAUVEGARDE / RECHARGEMENT DONNEES
	// =============================================
	private void LoadData()
	{
		Error err = configFile.Load("user://homescreen.cfg");
		if (err != Error.Ok) return;

		bool expertVisible = (bool)configFile.GetValue("UI", "ExpertButtonVisible", false);
		ExpertBtn.Visible = expertVisible;

		bool superMindVisible = (bool)configFile.GetValue("UI", "SuperMindTabVisible", false);
		if (superMindVisible) BotTabContainer.ShowSuperMindTab();
	}
}
