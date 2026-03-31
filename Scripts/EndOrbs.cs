using Godot;
using System;

public partial class EndOrbs : Control
{
	[Export] AnimationPlayer RayOrbAnim;
	
	[Export] MarginContainer EndOrbCollection;

	[Export] TextureRect EndOrbBlack;
	[Export] TextureRect EndOrbBlue;
	[Export] TextureRect EndOrbPurple;
	[Export] TextureRect EndOrbRed;
	[Export] TextureRect EndOrbWhite;
	[Export] TextureRect EndOrbYellow;
	
	[Export] TextureRect EverSlotBlack;
	[Export] TextureRect EverSlotBlue;
	[Export] TextureRect EverSlotPurple;
	[Export] TextureRect EverSlotRed;
	[Export] TextureRect EverSlotWhite;
	[Export] TextureRect EverSlotYellow;

	ConfigFile config = new ConfigFile();

	public override void _Ready()
	{
		ResetVisibility();
		ResetOrbSlotParticles();
		LoadData();
	}

	public void DisplayEndOrbCollection()
	{
		this.Visible = true;
		EndOrbCollection.Visible = true;
		config.SetValue("UI", "EndOrbCollectionUnlocked", true);
		config.Save("user://endorbs.cfg");
	}

	public void DisplayEndOrb(int endOrbEarned)
	{
		GD.Print("Orbe gagnée (0-5) = " + endOrbEarned);
		switch (endOrbEarned)
		{
			case 0: EndOrbBlack.Visible = true;  config.SetValue("Player", "EndOrbBlackUnlocked", true);  break;
			case 1: EndOrbBlue.Visible = true;   config.SetValue("Player", "EndOrbBlueUnlocked", true);   break;
			case 2: EndOrbPurple.Visible = true; config.SetValue("Player", "EndOrbPurpleUnlocked", true); break;
			case 3: EndOrbRed.Visible = true;    config.SetValue("Player", "EndOrbRedUnlocked", true);    break;
			case 4: EndOrbWhite.Visible = true;  config.SetValue("Player", "EndOrbWhiteUnlocked", true);  break;
			case 5: EndOrbYellow.Visible = true; config.SetValue("Player", "EndOrbYellowUnlocked", true); break;
		}
		config.Save("user://endorbs.cfg");
	}
	
	private void ResetVisibility()
	{
		RayOrbAnim.Play("RESET");
		
		EndOrbBlack.Visible = false;
		EndOrbBlue.Visible = false;
		EndOrbPurple.Visible = false;
		EndOrbRed.Visible = false;	
		EndOrbWhite.Visible = false;
		EndOrbYellow.Visible = false;

		EverSlotBlack.Visible = false;
		EverSlotBlue.Visible = false;
		EverSlotPurple.Visible = false;
		EverSlotRed.Visible = false;
		EverSlotWhite.Visible = false;
		EverSlotYellow.Visible = false;

		EndOrbCollection.Visible = false;
		this.Visible = false;
	}

	public void ResetOrbSlotParticles()
	{
		foreach (var node in GetTree().GetNodesInGroup("orb_slot_particles"))
		{
			if (node is TextureRect tr && tr.Material is CanvasItemMaterial mat)
			{
				mat.ParticlesAnimVFrames = 8;
			}
		}
	}

	public void ResetData()
	{
		var err = DirAccess.RemoveAbsolute("user://endorbs.cfg");
		var exists = FileAccess.FileExists("user://endorbs.cfg");
		config = new ConfigFile();
		ResetVisibility();
		ResetOrbSlotParticles();
	}
	
	private void LoadData()
	{
		var exists = FileAccess.FileExists("user://endorbs.cfg");
		Error err = config.Load("user://endorbs.cfg");
		if (err != Error.Ok)
		{
			ResetVisibility();
			return;
		}
		
		bool orbUnlocked = (bool)config.GetValue("UI", "EndOrbCollectionUnlocked", false);
		EndOrbCollection.Visible = orbUnlocked;
		this.Visible = orbUnlocked;

		bool displayBlack =  (bool)config.GetValue("Player", "EndOrbBlackUnlocked", false);
		bool displayBlue =   (bool)config.GetValue("Player", "EndOrbBlueUnlocked", false);
		bool displayPurple = (bool)config.GetValue("Player", "EndOrbPurpleUnlocked", false);
		bool displayRed =    (bool)config.GetValue("Player", "EndOrbRedUnlocked", false);
		bool displayWhite =  (bool)config.GetValue("Player", "EndOrbWhiteUnlocked", false);
		bool displayYellow = (bool)config.GetValue("Player", "EndOrbYellowUnlocked", false);
		
		EndOrbBlack.Visible = displayBlack;
		EverSlotBlack.Visible = displayBlack;
		
		EndOrbBlue.Visible = displayBlue;
		EverSlotBlue.Visible = displayBlue;
		
		EndOrbPurple.Visible = displayPurple;
		EverSlotPurple.Visible = displayPurple;
		
		EndOrbRed.Visible = displayRed;
		EverSlotRed.Visible = displayRed;
		
		EndOrbWhite.Visible = displayWhite;
		EverSlotWhite.Visible = displayWhite;
		
		EndOrbYellow.Visible = displayYellow;
		EverSlotYellow.Visible = displayYellow;
		
	}
}
