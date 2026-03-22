using Godot;
using System;
public partial class OrbSpawn : VBoxContainer
{
	[Export] OrbTexture OrbTexture;
	[Export] HomeScreen HomeScreen;
	[Export] EndOrbs EndOrbs;

	ConfigFile config = new ConfigFile();

	private bool _firstBlack = false;
	private bool _firstBlue = false;
	private bool _firstPurple = false;
	private bool _firstRed = false;
	private bool _firstWhite = false;
	private bool _firstYellow = false;
	public int orbCount = 0;

	public override void _Ready()
	{
		LoadData();
		GD.Print("orbCount au ready après Load Data dans OrbSpawn = " + orbCount);
	}

	public void SelectOrb(int randomOrb)
	{
		OrbTexture.Select(randomOrb);
		GD.Print("test pour voir si ça va jusque là");
	}

	public bool GetOrbInCollec(int randomOrb)
	{
		bool isNew = false;
		switch (randomOrb)
		{
			case 0: if (!_firstBlack)  { _firstBlack = true;  orbCount++; config.SetValue("Player", "BlackOrbObtained", true);  isNew = true; } break;
			case 1: if (!_firstBlue)   { _firstBlue = true;   orbCount++; config.SetValue("Player", "BlueOrbObtained", true);   isNew = true; } break;
			case 2: if (!_firstPurple) { _firstPurple = true; orbCount++; config.SetValue("Player", "PurpleOrbObtained", true); isNew = true; } break;
			case 3: if (!_firstRed)    { _firstRed = true;    orbCount++; config.SetValue("Player", "RedOrbObtained", true);    isNew = true; } break;
			case 4: if (!_firstWhite)  { _firstWhite = true;  orbCount++; config.SetValue("Player", "WhiteOrbObtained", true);  isNew = true; } break;
			case 5: if (!_firstYellow) { _firstYellow = true; orbCount++; config.SetValue("Player", "YellowOrbObtained", true); isNew = true; } break;
		}
		if (isNew)
		{
			HomeScreen.DisplayOrb(randomOrb);
			HomeScreen.DisplayOrbCollection();
			config.Save("user://orbspawn.cfg");
		}
		GD.Print("orbCount après GetOrbInCollec = " + orbCount);
		return isNew;
	}

	public void ResetData()
	{
		config = new ConfigFile();
		_firstBlack = _firstBlue = _firstPurple = _firstRed = _firstWhite = _firstYellow = false;
		orbCount = 0;
		_Ready();
	}

	private void LoadData()
	{
		Error err = config.Load("user://orbspawn.cfg");
		if (err != Error.Ok) return;

		_firstBlack =   (bool)config.GetValue("Player", "BlackOrbObtained", false);
		_firstBlue =    (bool)config.GetValue("Player", "BlueOrbObtained", false);
		_firstPurple =  (bool)config.GetValue("Player", "PurpleOrbObtained", false);
		_firstRed =     (bool)config.GetValue("Player", "RedOrbObtained", false);
		_firstWhite =   (bool)config.GetValue("Player", "WhiteOrbObtained", false);
		_firstYellow =  (bool)config.GetValue("Player", "YellowOrbObtained", false);

		orbCount = 0;
		if (_firstBlack) orbCount++;
		if (_firstBlue) orbCount++;
		if (_firstPurple) orbCount++;
		if (_firstRed) orbCount++;
		if (_firstWhite) orbCount++;
		if (_firstYellow) orbCount++;

		GD.Print("orbCount chargé = " + orbCount);
	}
}
