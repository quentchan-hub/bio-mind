using Godot;
using System;

public partial class TestsDebug : Control
{
	[Export] Brain Brain;
	[Export] HomeScreen HomeScreen;
	[Export] EndScreen EndScreen;
	[Export] OrbSpawn OrbSpawn;
	[Export] RobotPartSpawn RobotPartSpawn;
	
	private ConfigFile config = new ConfigFile();

	//===============================================================
	// TEST TEST TEST
	//===============================================================
	private void _on_get_all_orbs_pressed()
	{
		for (int i = 0; i < 6; i++)
			OrbSpawn.GetOrbInCollec(i);

		OrbSpawn.orbCount = 6;
		EndScreen._isSixOrbsObtained = true;
		config.SetValue("Player", "IsSixOrbsObtained", true);
		config.Save("user://endscreen.cfg");
		HomeScreen.OnSixOrbsCompleted();
		GD.Print("Fin du test TestGetAllOrbs orbCount = " + OrbSpawn.orbCount);
	}

	private void _on_orb_count_pressed()
	{
		GD.Print("orbCount actuel = " + OrbSpawn.orbCount);
	}

	private void _on_get_all_parts_pressed()
	{
		for (int i = 0; i < 8; i++)
			RobotPartSpawn.GetPartInCollec(i);

		RobotPartSpawn.partCount = 8;
		EndScreen._isEightPartsObtained = true;
		config.SetValue("Player", "IsEightPartsObtained", true);
		config.Save("user://endscreen.cfg");
		HomeScreen.OnEightPartsCompleted();
		GD.Print("Fin du test TestGetAllParts partCount = " + RobotPartSpawn.partCount);
	}

	private void _on_part_count_pressed()
	{
		GD.Print("partCount actuel = " + RobotPartSpawn.partCount);
	}
	
	private void _on_solution_pressed()
	{
		Brain.CheckRow(Brain._solution);
	}
	
}
