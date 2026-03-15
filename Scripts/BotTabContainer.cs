using Godot;
using System;

public partial class BotTabContainer : TabContainer
{
	[Export] Control Rules;
	
	private const int TabRules = 0;
	private const int TabOrbs = 1;
	private const int TabParts = 2;
	private const int TabSuperMind = 3;

	private readonly ConfigFile config = new ConfigFile();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetTabTitle(0, "Regles"); // index de l'onglet, titre
		SetTabTitle(1, "Orbes");
		SetTabTitle(2, "Montage");
		SetTabTitle(3, "Robot");
		
		LoadData();
	}
	
	public void HidePartsTab()
	{
		SetTabHidden(2, true); // cache l'onglet Montage au demarrage
		config.SetValue("UI", "ShowPartsTab", false);
		SaveData();
		Rules.Visible = true;
	}
	
	public void ShowPartsTab()
	{
		SetTabHidden(2, false); // reaffiche l'onglet si cache
		config.SetValue("UI", "ShowPartsTab", true);
		SaveData();
	}
	
	public void HideSuperMindTab()
	{
		SetTabHidden(3, true); // cache l'onglet SuperMind au demarrage
		config.SetValue("UI", "ShowSuperMindTab", false);
		SaveData();
		Rules.Visible = true;
	}
	
	public void ShowSuperMindTab()
	{
		SetTabHidden(3, false); // reaffiche l'onglet si cache
		config.SetValue("UI", "ShowSuperMindTab", true);
		SaveData();
	}
	
	private void SaveData()
	{
		config.Save("user://bottabcontainer.cfg");
	}
	
	private void LoadData()
	{
		Error err = config.Load("user://bottabcontainer.cfg");
		if (err != Error.Ok)
		{
			SetTabHidden(2, true);
			SetTabHidden(3, true);
			Rules.Visible = true;
			return;
		}
		
		bool showPartsTab = (bool)config.GetValue("UI", "ShowPartsTab", false);
		bool showSuperMindTab = (bool)config.GetValue("UI", "ShowSuperMindTab", false);
		
		SetTabHidden(2, !showPartsTab);
		SetTabHidden(3, !showSuperMindTab);
	}
}
