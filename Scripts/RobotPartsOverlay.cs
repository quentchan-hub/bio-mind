using Godot;
using System;

public partial class RobotPartsOverlay : Control
{
	[Export] VBoxContainer LeftArmZoom;
	[Export] VBoxContainer RightArmZoom;
	[Export] VBoxContainer LeftLegZoom;
	[Export] VBoxContainer RightLegZoom;
	[Export] VBoxContainer ChestZoom;
	[Export] VBoxContainer HeadZoom;
	[Export] VBoxContainer SwordZoom;
	[Export] VBoxContainer ShieldZoom;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
		
		LeftArmZoom.Visible = false;
		RightArmZoom.Visible = false;
		LeftLegZoom.Visible = false;
		RightLegZoom.Visible = false;
		HeadZoom.Visible = false;
		ChestZoom.Visible = false;
		SwordZoom.Visible = false;
		ShieldZoom.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_close_robot_button_pressed()
	{
		Visible = false;
	}
	
	private void _on_left_arm_button_pressed()
	{
		LeftArmZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_right_arm_button_pressed()
	{
		RightArmZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_left_leg_button_pressed()
	{
		LeftLegZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_right_leg_button_pressed()
	{
		RightLegZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_head_button_pressed()
	{
		HeadZoom.Visible = true;
		this.Visible = true;
	}

	private void _on_chest_button_pressed()
	{
		ChestZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_sword_button_pressed()
	{
		SwordZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_shield_button_pressed()
	{
		ShieldZoom.Visible = true;
		this.Visible = true;
	}
}
