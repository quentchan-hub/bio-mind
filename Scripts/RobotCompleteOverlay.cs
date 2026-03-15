using Godot;
using System;

public partial class RobotCompleteOverlay : Control
{
	[Export] VBoxContainer SuperMindNWZoom;
	[Export] VBoxContainer SuperMindSASZoom;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
		
		SuperMindNWZoom.Visible = false;
		SuperMindSASZoom.Visible = false;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_close_robot_button_pressed()
	{
		Visible = false;
	}
	
	private void _on_super_mind_nw_pressed()
	{
		
		SuperMindNWZoom.Visible = true;
		SuperMindSASZoom.Visible = false;
		this.Visible = true;
	}
	
	private void _on_super_mind_sas_pressed()
	{
		
		SuperMindNWZoom.Visible = false;
		SuperMindSASZoom.Visible = true;
		this.Visible = true;
	}
	
}
