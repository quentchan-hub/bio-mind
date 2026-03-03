using Godot;
using System;

public partial class HomeScreen : Control
{
	[Signal] public delegate void StartGameEventHandler();
	
	[Export] Brain Brain;
	[Export] Control OptionsOverlay;
	
	// ready
	public override void _Ready()
	{
		OptionsOverlay.Visible = false;
	}

	// process
	public override void _Process(double delta)
	{
	}
	
	private void _on_options_btn_pressed()
	{
		OptionsOverlay.Visible = true;
	}
	
	private void _on_close_options_button_pressed()
	{
		OptionsOverlay.Visible = false;
	}
}
