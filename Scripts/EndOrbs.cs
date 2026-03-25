using Godot;
using System;

public partial class EndOrbs : Control
{
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
		EndOrbBlack.Visible = false;
		EndOrbBlue.Visible = false;
		EndOrbPurple.Visible = false;
		EndOrbRed.Visible = false;
		EndOrbWhite.Visible = false;
		EndOrbYellow.Visible = false;
		EndOrbCollection.Visible = false;
		
		EverSlotBlack.Visible = false;
		EverSlotBlue.Visible = false;
		EverSlotPurple.Visible = false;
		EverSlotRed.Visible = false;
		EverSlotWhite.Visible = false;
		EverSlotYellow.Visible = false;
		
		LoadData();
		
		GD.Print(
			"Dans Ready de EndOrbs après LoadData() : " +
			$"Black={EndOrbBlack.Visible}, " +
			$"Blue={EndOrbBlue.Visible}, " +
			$"Purple={EndOrbPurple.Visible}, " +
			$"Red={EndOrbRed.Visible}, " +
			$"White={EndOrbWhite.Visible}, " +
			$"Yellow={EndOrbYellow.Visible}, " +
			$"Collection={EndOrbCollection.Visible}"
		);
		
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

	public void ResetData()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("user://endorbs.cfg");
		config = new ConfigFile();
		_Ready();
	}

	private void LoadData()
	{
		Error err = config.Load("user://endorbs.cfg");
		if (err != Error.Ok) return;

		bool orbUnlocked = (bool)config.GetValue("UI", "EndOrbCollectionUnlocked", false);
		EndOrbCollection.Visible = orbUnlocked;
		this.Visible = orbUnlocked;

		EndOrbBlack.Visible =  (bool)config.GetValue("Player", "EndOrbBlackUnlocked", false);
		EndOrbBlue.Visible =   (bool)config.GetValue("Player", "EndOrbBlueUnlocked", false);
		EndOrbPurple.Visible = (bool)config.GetValue("Player", "EndOrbPurpleUnlocked", false);
		EndOrbRed.Visible =    (bool)config.GetValue("Player", "EndOrbRedUnlocked", false);
		EndOrbWhite.Visible =  (bool)config.GetValue("Player", "EndOrbWhiteUnlocked", false);
		EndOrbYellow.Visible = (bool)config.GetValue("Player", "EndOrbYellowUnlocked", false);
		
		EverSlotBlack.Visible = EndOrbBlack.Visible;
		EverSlotBlue.Visible = EndOrbBlue.Visible;
		EverSlotPurple.Visible = EndOrbPurple.Visible;
		EverSlotRed.Visible = EndOrbRed.Visible;
		EverSlotWhite.Visible = EndOrbWhite.Visible;
		EverSlotYellow.Visible = EndOrbYellow.Visible;
		
		GD.Print(
			"Dans LoadData() : " +
			$"Black={EndOrbBlack.Visible}, " +
			$"Blue={EndOrbBlue.Visible}, " +
			$"Purple={EndOrbPurple.Visible}, " +
			$"Red={EndOrbRed.Visible}, " +
			$"White={EndOrbWhite.Visible}, " +
			$"Yellow={EndOrbYellow.Visible}, " +
			$"Collection={EndOrbCollection.Visible}"
		);
	}
}
