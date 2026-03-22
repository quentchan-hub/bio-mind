using Godot;
using System;
public partial class HomeScreen : Control
{
	[Signal] public delegate void StartGameEventHandler();

	[Export] Brain Brain;
	[Export] EndScreen EndScreen;
	[Export] Orbs Orbs;
	[Export] Parts Parts;
	[Export] Poses Poses;
	[Export] OrbSpawn OrbSpawn;
	[Export] RobotPartSpawn RobotPartSpawn;
	[Export] Control DifficultyOverlay;
	
	[Export] Control StatsOverlay;
	[Export] ConfirmationDialog ConfirmResetStat;
	
	[Export] Control CollectionsOverlay;
	[Export] ConfirmationDialog ConfirmResetCollec;
	
	ConfigFile configFile = new ConfigFile();

	public override void _Ready()
	{
		DifficultyOverlay.Visible = false;
		
		StatsOverlay.Visible = false;
		ConfirmResetStat.Visible = false;
		
		CollectionsOverlay.Visible = false;
		ConfirmResetCollec.Visible = false;
		
		LoadData();
	}
	
	// DifficultyOverlay = GameLauncher
	private void _on_launch_btn_pressed() => DifficultyOverlay.Visible = true;
	
	// StatsOverlay
	private void _on_stat_button_pressed() => StatsOverlay.Visible = true;
	private void _on_reset_stat_button_pressed() => ConfirmResetStat.Visible = true;
	
	// CollectionsOverlay
	private void _on_collec_button_pressed() => CollectionsOverlay.Visible = true;
	private void _on_reset_collections_button_pressed() => ConfirmResetCollec.Visible = true;
	private void _on_confirm_reset_collec_confirmed()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("homescreen.cfg");
		dir.Remove("orbs.cfg"); // dans CollectionsOverlay
		dir.Remove("parts.cfg"); // dans CollectionsOverlay
		
		dir.Remove("endscreen.cfg");
		dir.Remove("orbspawn.cfg"); // dans EndScreen
		dir.Remove("robotpartspawn.cfg"); // dans EndScreen

		configFile = new ConfigFile();
		OrbSpawn.ResetData();
		Orbs.ResetData();
		Parts.ResetData();
		RobotPartSpawn.ResetData();
		EndScreen.ResetData();
		
		OrbSpawn.orbCount = 0;
		RobotPartSpawn.partCount = 0;

		ConfirmResetCollec.Visible = false;
		
		GD.Print(" après reset orbCount = " + OrbSpawn.orbCount);
		GD.Print(" après reset partCount = " + RobotPartSpawn.partCount);
	}
	

	// ============================
	//      ORB COLLECTION
	// ============================
	// -- Collecting --
	public void DisplayOrbCollection()
	{
		Orbs.DisplayOrbCollection();
		Parts.UnveilPartCollection();
	}	
	public void DisplayOrb(int orbEarned) => Orbs.DisplayOrb(orbEarned);

	// -- Orb Collection complete --
	public void OnSixOrbsCompleted()
	{
		//ExpertBtn.Visible = true;
		//configFile.SetValue("UI", "ExpertButtonVisible", true);
		//configFile.Save("user://homescreen.cfg");
	}

	// ============================
	//    ROBOT PARTS COLLECTION
	// ============================
	// -- Collecting --
	public void DisplayPartCollection()
	{
		Parts.DisplayPartCollection();
		Poses.UnveilRobotCollection();
	}
	public void DisplayPart(int partEarned) => Parts.DisplayPart(partEarned);
	
	// -- Part Collection complete --
	public void OnEightPartsCompleted() 
	{
		//configFile.SetValue("UI", "SuperMindTabVisible", true);
		//configFile.Save("user://homescreen.cfg");
	} 

	// =============================================
	//   GESTION SAUVEGARDE / RECHARGEMENT DONNEES
	// =============================================
	private void LoadData()
	{
		Error err = configFile.Load("user://homescreen.cfg");
		if (err != Error.Ok) return;

		//bool expertVisible = (bool)configFile.GetValue("UI", "ExpertButtonVisible", false);
		//ExpertBtn.Visible = expertVisible;

		//bool superMindVisible = (bool)configFile.GetValue("UI", "SuperMindTabVisible", false);
		//if (superMindVisible) BotTabContainer.ShowSuperMindTab();
	}
}
