using Godot;
using System;

public partial class Poses : Control
{
	[Export] PanelContainer DisplayRobotCollecInstruct;
	[Export] PanelContainer OverVeil;
	[Export] Label CollecInstructLabel;
	[Export] Label RobotTitle;
	[Export] Label RobotTitleCrypt;
	
	[Export] TextureButton SuperMindNW;
	[Export] TextureButton SuperMindSAS;

	ConfigFile config = new ConfigFile();

	public override void _Ready()
	{
		//SuperMindNW.Visible = false;
		//SuperMindSAS.Visible = false;

		DisplayRobotCollecInstruct.Visible = true;
		CollecInstructLabel.Visible = false;
		OverVeil.Visible = true;
		RobotTitle.Visible = false;
		RobotTitleCrypt.Visible = true;
		
		LoadData();
	}

	public void UnveilRobotCollection()
	{
		OverVeil.Visible = false;
		CollecInstructLabel.Visible = true;
		RobotTitle.Visible = true;
		RobotTitleCrypt.Visible = false;
		
		config.SetValue("UI", "RobotCollectionUnveiled", true);
		config.Save("user://robot.cfg");
	}
	
	public void DisplayRobotCollection()
	{
		DisplayRobotCollecInstruct.Visible = false;
		config.SetValue("UI", "RobotCollectionUnveiled", true);
		config.Save("user://robot.cfg");
	}

	public void DisplayPose(int poseEarned)
	{
		//GD.Print("Pose robot gagnée (0-1) = " + poseEarned);
		//switch (poseEarned)
		//{
			//case 0: SuperMindNW.Visible = true;  config.SetValue("Player", "SuperMindNW", true);  break;
			//case 1: SuperMindSAS.Visible = true; config.SetValue("Player", "SuperMindSAS", true); break;
		//}
		//config.Save("user://robot.cfg");
	}

	public void ResetData()
	{
		config = new ConfigFile();
		_Ready();
	}

	private void LoadData()
	{
		Error err = config.Load("user://parts.cfg");
		if (err != Error.Ok) return;
		
		bool robotUnveiled = (bool)config.GetValue("UI", "RobotCollectionUnveiled", false);
		OverVeil.Visible = !robotUnveiled;
		CollecInstructLabel.Visible = robotUnveiled;
		RobotTitle.Visible = robotUnveiled;
		RobotTitleCrypt.Visible = !robotUnveiled;
		
		
		//bool robotUnlocked = (bool)config.GetValue("UI", "PartCollectionUnlocked", false);
		//RobotPoseCollection.Visible = partUnlocked;
		//DisplayRobotCollecInstruct.Visible = !partUnlocked;

		SuperMindNW.Visible =  (bool)config.GetValue("Player", "SuperMindNW", false);
		SuperMindSAS.Visible = (bool)config.GetValue("Player", "SuperMindSAS", false);

	}
}
