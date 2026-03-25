using Godot;
using System;

public partial class DifficultyOverlay : Control
{
	public bool IsChronoEnabled = false;

	public override void _Ready()
	{
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

	private void LoadData()
	{
		// reserved
	}

	public void ResetData()
	{
		_Ready();
	}
}
