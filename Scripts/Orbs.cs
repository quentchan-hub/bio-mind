using Godot;
using System;
public partial class Orbs : Control
{
	[Export] PanelContainer DisplayCollecInstruct;
	[Export] MarginContainer OrbCollection;

	[Export] TextureRect OrbBlack;
	[Export] TextureRect OrbBlue;
	[Export] TextureRect OrbPurple;
	[Export] TextureRect OrbRed;
	[Export] TextureRect OrbWhite;
	[Export] TextureRect OrbYellow;

	ConfigFile config = new ConfigFile();

	public override void _Ready()
	{
		OrbBlack.Visible = false;
		OrbBlue.Visible = false;
		OrbPurple.Visible = false;
		OrbRed.Visible = false;
		OrbWhite.Visible = false;
		OrbYellow.Visible = false;
		OrbCollection.Visible = false;
		DisplayCollecInstruct.Visible = true;

		LoadData();
	}

	public void DisplayOrbCollection()
	{
		this.Visible = true;
		OrbCollection.Visible = true;
		DisplayCollecInstruct.Visible = false;
		config.SetValue("UI", "OrbCollectionUnlocked", true);
		config.Save("user://orbs.cfg");
	}

	public void DisplayOrb(int orbEarned)
	{
		//GD.Print("Orbe gagnée (0-5) = " + orbEarned);
		switch (orbEarned)
		{
			case 0: OrbBlack.Visible = true;  config.SetValue("Player", "OrbBlackUnlocked", true);  break;
			case 1: OrbBlue.Visible = true;   config.SetValue("Player", "OrbBlueUnlocked", true);   break;
			case 2: OrbPurple.Visible = true;  config.SetValue("Player", "OrbPurpleUnlocked", true); break;
			case 3: OrbRed.Visible = true;    config.SetValue("Player", "OrbRedUnlocked", true);    break;
			case 4: OrbWhite.Visible = true;  config.SetValue("Player", "OrbWhiteUnlocked", true);  break;
			case 5: OrbYellow.Visible = true; config.SetValue("Player", "OrbYellowUnlocked", true); break;
		}
		config.Save("user://orbs.cfg");
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

		bool orbUnlocked = (bool)config.GetValue("UI", "OrbCollectionUnlocked", false);
		OrbCollection.Visible = orbUnlocked;
		DisplayCollecInstruct.Visible = !orbUnlocked;

		OrbBlack.Visible =  (bool)config.GetValue("Player", "OrbBlackUnlocked", false);
		OrbBlue.Visible =   (bool)config.GetValue("Player", "OrbBlueUnlocked", false);
		OrbPurple.Visible = (bool)config.GetValue("Player", "OrbPurpleUnlocked", false);
		OrbRed.Visible =    (bool)config.GetValue("Player", "OrbRedUnlocked", false);
		OrbWhite.Visible =  (bool)config.GetValue("Player", "OrbWhiteUnlocked", false);
		OrbYellow.Visible = (bool)config.GetValue("Player", "OrbYellowUnlocked", false);
	}
}
