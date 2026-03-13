using Godot;
using System;
public partial class HomeScreen : Control
{
	[Signal] public delegate void StartGameEventHandler();

	[Export] Brain Brain;
	[Export] EndScreen EndScreen;
	[Export] BotTabContainer BotTabContainer;
	[Export] Control OptionsOverlay;
	[Export] ConfirmationDialog ConfirmReset;
	[Export] Orbs Orbs;
	[Export] SuperMind SuperMind;
	[Export] Button ExpertBtn;

	ConfigFile configFile = new ConfigFile();

	public override void _Ready()
	{
		OptionsOverlay.Visible = false;
		ConfirmReset.Visible = false;
		ExpertBtn.Visible = false;
		LoadData();
	}

	// ===========================
	//     MENU OPTIONS
	// ===========================
	private void _on_options_btn_pressed() => OptionsOverlay.Visible = true;
	private void _on_close_options_button_pressed() => OptionsOverlay.Visible = false;
	private void _on_reset_all_button_pressed() => ConfirmReset.Visible = true;

	private void _on_confirmation_dialog_confirmed()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("homescreen.cfg");
		dir.Remove("endscreen.cfg");
		dir.Remove("orbs.cfg");
		dir.Remove("supermind.cfg");

		configFile = new ConfigFile();
		EndScreen.config = new ConfigFile();

		Orbs.ResetData();
		SuperMind.ResetData();
		BotTabContainer.HideSuperMindTab();
		EndScreen._Ready();

		ExpertBtn.Visible = false;
		OptionsOverlay.Visible = false;
		ConfirmReset.Visible = false;
	}

	// ============================
	//      ORB COLLECTION
	// ============================
	public void DisplayOrbCollection() => Orbs.DisplayOrbCollection();
	public void DisplayOrb(int orbEarned) => Orbs.DisplayOrb(orbEarned);

	public void OnSixOrbsCompleted()
	{
		ExpertBtn.Visible = true;
		configFile.SetValue("UI", "ExpertButtonVisible", true);
		configFile.Save("user://homescreen.cfg");
		BotTabContainer.ShowSuperMindTab();
	}

	// ============================
	//    ROBOT PARTS COLLECTION
	// ============================
	public void DisplayPartCollection() => SuperMind.DisplayPartCollection();
	public void DisplayPart(int partEarned) => SuperMind.DisplayPart(partEarned);
	public void OnEightPartsCompleted() { } // A définir

	// =============================================
	//   GESTION SAUVEGARDE / RECHARGEMENT DONNEES
	// =============================================
	private void LoadData()
	{
		Error err = configFile.Load("user://homescreen.cfg");
		if (err != Error.Ok) return;

		bool expertVisible = (bool)configFile.GetValue("UI", "ExpertButtonVisible", false);
		ExpertBtn.Visible = expertVisible;
	}
}
