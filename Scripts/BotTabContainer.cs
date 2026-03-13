using Godot;
using System;

public partial class BotTabContainer : TabContainer
{
	ConfigFile config = new ConfigFile();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetTabTitle(0, "Règles"); // index de l'onglet, titre
		SetTabTitle(1, "Orbes");
		SetTabTitle(2, "SuperMind");
		
		LoadData();
	}
	
	
	public void HideSuperMindTab()
	{
		SetTabHidden(2, true); // cache l'onglet SuperMind au démarrage
		config.SetValue("UI", "ShowSuperMindTab", false);
		SaveData();
	}
	
	public void ShowSuperMindTab()
	{
		SetTabHidden(2, false); // réaffiche l'onglet si caché
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
		if (err != Error.Ok) return;
			
		bool showSuperMindTab = false;
		showSuperMindTab = (bool)config.GetValue("UI", "ShowSuperMindTab", false);
		
		if (showSuperMindTab) ShowSuperMindTab();
	}
}
