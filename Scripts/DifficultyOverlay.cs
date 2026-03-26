using Godot;
using System;

public partial class DifficultyOverlay : Control
{
	[Export] PanelContainer BlockExpertMode;
	[Export] CollectionsOverlay CollectionsOverlay;
	
	public bool IsChronoEnabled = false;
	
	private ConfigFile _config = new ConfigFile();

	public override void _Ready()
	{
		CollectionsOverlay.OnResetDifficultyProgress += ResetData;
		
		BlockExpertMode.Visible = true;
		Visible = false;
		LoadData();
	}

	private void _on_check_button_toggled(bool toggledOn)
	{
		IsChronoEnabled = toggledOn;
	}

	private void _on_play_btn_pressed()
	{
		Visible = false;
	}
	
	public void HideBlockExpertMode()
	{
		BlockExpertMode.Visible = false;
		_config.SetValue("UI", "ShowBlockExpertMode", false);
		_config.Save("user://difficultyoverlay.cfg");
	}
	
	private void LoadData()
	{
		Error err = _config.Load("user://difficultyoverlay.cfg");
		if (err != Error.Ok) return;
		
		BlockExpertMode.Visible = (bool)_config.GetValue("UI", "ShowBlockExpertMode", false);
	}

	public void ResetData()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("difficultyoverlay.cfg");
		_config = new ConfigFile();
		_Ready();
	}
}
