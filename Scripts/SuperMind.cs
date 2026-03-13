using Godot;
using System;
public partial class SuperMind : Control
{
	[Export] PanelContainer DisplayRobotCollecInstruct;
	[Export] MarginContainer PartCollection;

	[Export] TextureButton LeftArmButton;
	[Export] TextureButton RightArmButton;
	[Export] TextureButton LeftLegButton;
	[Export] TextureButton RightLegButton;
	[Export] TextureButton HeadButton;
	[Export] TextureButton ChestButton;
	[Export] TextureButton SwordButton;
	[Export] TextureButton ShieldButton;

	ConfigFile config = new ConfigFile();

	public override void _Ready()
	{
		LeftArmButton.Visible = false;
		RightArmButton.Visible = false;
		LeftLegButton.Visible = false;
		RightLegButton.Visible = false;
		HeadButton.Visible = false;
		ChestButton.Visible = false;
		SwordButton.Visible = false;
		ShieldButton.Visible = false;
		PartCollection.Visible = false;
		DisplayRobotCollecInstruct.Visible = true;

		LoadData();
	}

	public void DisplayPartCollection()
	{
		PartCollection.Visible = true;
		DisplayRobotCollecInstruct.Visible = false;
		config.SetValue("UI", "PartCollectionUnlocked", true);
		config.Save("user://supermind.cfg");
	}

	public void DisplayPart(int partEarned)
	{
		GD.Print("Partie robot gagnée (0-7) = " + partEarned);
		switch (partEarned)
		{
			case 0: LeftArmButton.Visible = true;  config.SetValue("Player", "LeftArmUnlocked", true);  break;
			case 1: RightArmButton.Visible = true; config.SetValue("Player", "RightArmUnlocked", true); break;
			case 2: LeftLegButton.Visible = true;  config.SetValue("Player", "LeftLegUnlocked", true);  break;
			case 3: RightLegButton.Visible = true; config.SetValue("Player", "RightLegUnlocked", true); break;
			case 4: HeadButton.Visible = true;     config.SetValue("Player", "HeadUnlocked", true);     break;
			case 5: ChestButton.Visible = true;    config.SetValue("Player", "ChestUnlocked", true);    break;
			case 6: SwordButton.Visible = true;    config.SetValue("Player", "SwordUnlocked", true);    break;
			case 7: ShieldButton.Visible = true;   config.SetValue("Player", "ShieldUnlocked", true);   break;
		}
		config.Save("user://supermind.cfg");
	}

	public void ResetData()
	{
		config = new ConfigFile();
		_Ready();
	}

	private void LoadData()
	{
		Error err = config.Load("user://supermind.cfg");
		if (err != Error.Ok) return;

		bool partUnlocked = (bool)config.GetValue("UI", "PartCollectionUnlocked", false);
		PartCollection.Visible = partUnlocked;
		DisplayRobotCollecInstruct.Visible = !partUnlocked;

		LeftArmButton.Visible =  (bool)config.GetValue("Player", "LeftArmUnlocked", false);
		RightArmButton.Visible = (bool)config.GetValue("Player", "RightArmUnlocked", false);
		LeftLegButton.Visible =  (bool)config.GetValue("Player", "LeftLegUnlocked", false);
		RightLegButton.Visible = (bool)config.GetValue("Player", "RightLegUnlocked", false);
		HeadButton.Visible =     (bool)config.GetValue("Player", "HeadUnlocked", false);
		ChestButton.Visible =    (bool)config.GetValue("Player", "ChestUnlocked", false);
		SwordButton.Visible =    (bool)config.GetValue("Player", "SwordUnlocked", false);
		ShieldButton.Visible =   (bool)config.GetValue("Player", "ShieldUnlocked", false);
	}
}
