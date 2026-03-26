using Godot;
using System;

public partial class CollectionsOverlay : Control
{
	[Signal] public delegate void OnResetDifficultyProgressEventHandler();
	
	[Export] Orbs Orbs;
	[Export] Parts Parts;
	[Export] Poses Poses;
	[Export] ConfirmationDialog ConfirmResetCollec;
	[Export] EndScreen EndScreen;

	private ConfigFile _config = new ConfigFile();

	public override void _Ready()
	{
		Visible = false;
		ConfirmResetCollec.Visible = false;
		LoadData();
	}
	
	// ============================
	//      ORB COLLECTION
	// ============================
	
	// -- Affichage emplacements --
		public void DisplayOrbCollection()
	{
		// on affiche les emplacements d'orbes
		Orbs.DisplayOrbCollection();
		// on lève le voile crypté, mais les emplacement restent cachés
		Parts.UnveilPartCollection();
	}	
	
	// -- Collecting --
	public void DisplayOrb(int orbEarned) => Orbs.DisplayOrb(orbEarned);

	// -- Orb Collection complete --
	public void OnSixOrbsCompleted()
	{
		DisplayPartCollection();
	}


	// ============================
	//    ROBOT PARTS COLLECTION
	// ============================
	
	// -- Affichage emplacements --
	public void DisplayPartCollection()
	{
		Parts.DisplayPartCollection();
		Poses.UnveilRobotCollection();
	}
	
	// -- Collecting Parts --
	public void DisplayPart(int partEarned) => Parts.DisplayPart(partEarned);
	
	// -- Part Collection complete --
	public void OnEightPartsCompleted() 
	{
		DisplayRobotCollection();
	} 
	
	// ============================
	//    ROBOT POSES COLLECTION
	// ============================
	
	// -- Affichage emplacements --
	public void DisplayRobotCollection() => Poses.DisplayRobotCollection();
	
	// -- Collecting Poses --
	public void DisplayPose(int poseEarned) => Poses.DisplayPose(poseEarned);
	
	// -- Pose Collection complete --
	public void OnTwoPartsCompleted() 
	{
		// au cas où
	} 
	
	private void _on_reset_collections_button_pressed() => ConfirmResetCollec.Visible = true;

	private void _on_confirm_reset_collec_confirmed()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("orbs.cfg");
		dir.Remove("parts.cfg");

		Orbs.ResetData();
		Parts.ResetData();
		Poses.ResetData();
		
		EmitSignal(SignalName.OnResetDifficultyProgress); 
		
		EndScreen.ResetEndScreenAndRewards();

		ConfirmResetCollec.Visible = false;
	}

	public void ResetData()
	{
		_config = new ConfigFile();
		_Ready();
	}

	private void LoadData()
	{
		// reserved
	}
}
