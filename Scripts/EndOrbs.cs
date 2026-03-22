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

		LoadData();
	}

	public void DisplayEndOrbCollection()
	{
		this.Visible = true;
		EndOrbCollection.Visible = true;
		config.SetValue("UI", "OrbEndCollectionUnlocked", true);
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
		config = new ConfigFile();
		_Ready();
	}

	private void LoadData()
	{
		Error err = config.Load("user://orbs.cfg");
		if (err != Error.Ok) return;

		bool orbUnlocked = (bool)config.GetValue("UI", "EndOrbCollectionUnlocked", false);
		EndOrbCollection.Visible = orbUnlocked;

		EndOrbBlack.Visible =  (bool)config.GetValue("Player", "EndOrbBlackUnlocked", false);
		EndOrbBlue.Visible =   (bool)config.GetValue("Player", "EndOrbBlueUnlocked", false);
		EndOrbPurple.Visible = (bool)config.GetValue("Player", "EndOrbPurpleUnlocked", false);
		EndOrbRed.Visible =    (bool)config.GetValue("Player", "EndOrbRedUnlocked", false);
		EndOrbWhite.Visible =  (bool)config.GetValue("Player", "EndOrbWhiteUnlocked", false);
		EndOrbYellow.Visible = (bool)config.GetValue("Player", "EndOrbYellowUnlocked", false);
	}
}
