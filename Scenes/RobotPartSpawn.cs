using Godot;
using System;
public partial class RobotPartSpawn : VBoxContainer
{
	[Export] PartTexture PartTexture;
	[Export] HomeScreen HomeScreen;

	ConfigFile config = new ConfigFile();

	private bool _firstLeftArm = false;
	private bool _firstRightArm = false;
	private bool _firstLeftLeg = false;
	private bool _firstRightLeg = false;
	private bool _firstHead = false;
	private bool _firstChest = false;
	private bool _firstSword = false;
	private bool _firstShield = false;
	public int partCount = 0;

	public override void _Ready()
	{
		LoadData();
	}

	public void SelectPart(int randomPart)
	{
		PartTexture.Select(randomPart);
	}

	public bool GetPartInCollec(int randomPart)
	{
		bool isNew = false;
		switch (randomPart)
		{
			case 0: if (!_firstLeftArm)  { _firstLeftArm = true;  partCount++; config.SetValue("Player", "LeftArmObtained", true);  isNew = true; } break;
			case 1: if (!_firstRightArm) { _firstRightArm = true; partCount++; config.SetValue("Player", "RightArmObtained", true); isNew = true; } break;
			case 2: if (!_firstLeftLeg)  { _firstLeftLeg = true;  partCount++; config.SetValue("Player", "LeftLegObtained", true);  isNew = true; } break;
			case 3: if (!_firstRightLeg) { _firstRightLeg = true; partCount++; config.SetValue("Player", "RightLegObtained", true); isNew = true; } break;
			case 4: if (!_firstHead)     { _firstHead = true;     partCount++; config.SetValue("Player", "HeadObtained", true);     isNew = true; } break;
			case 5: if (!_firstChest)    { _firstChest = true;    partCount++; config.SetValue("Player", "ChestObtained", true);    isNew = true; } break;
			case 6: if (!_firstSword)    { _firstSword = true;    partCount++; config.SetValue("Player", "SwordObtained", true);    isNew = true; } break;
			case 7: if (!_firstShield)   { _firstShield = true;   partCount++; config.SetValue("Player", "ShieldObtained", true);   isNew = true; } break;
		}
		if (isNew)
		{
			HomeScreen.DisplayPart(randomPart);
			HomeScreen.DisplayPartCollection();
			config.Save("user://robotpartspawn.cfg");
		}
		GD.Print("partCount après GetPartInCollec = " + partCount);
		return isNew;
	}

	public void ResetData()
	{
		config = new ConfigFile();
		_firstLeftArm = _firstRightArm = _firstLeftLeg = _firstRightLeg = false;
		_firstHead = _firstChest = _firstSword = _firstShield = false;
		partCount = 0;
		_Ready();
	}

	private void LoadData()
	{
		Error err = config.Load("user://robotpartspawn.cfg");
		if (err != Error.Ok) return;

		_firstLeftArm =  (bool)config.GetValue("Player", "LeftArmObtained", false);
		_firstRightArm = (bool)config.GetValue("Player", "RightArmObtained", false);
		_firstLeftLeg =  (bool)config.GetValue("Player", "LeftLegObtained", false);
		_firstRightLeg = (bool)config.GetValue("Player", "RightLegObtained", false);
		_firstHead =     (bool)config.GetValue("Player", "HeadObtained", false);
		_firstChest =    (bool)config.GetValue("Player", "ChestObtained", false);
		_firstSword =    (bool)config.GetValue("Player", "SwordObtained", false);
		_firstShield =   (bool)config.GetValue("Player", "ShieldObtained", false);

		partCount = 0;
		if (_firstLeftArm) partCount++;
		if (_firstRightArm) partCount++;
		if (_firstLeftLeg) partCount++;
		if (_firstRightLeg) partCount++;
		if (_firstHead) partCount++;
		if (_firstChest) partCount++;
		if (_firstSword) partCount++;
		if (_firstShield) partCount++;

		GD.Print("partCount chargé = " + partCount);
	}
}
