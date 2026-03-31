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
		
		HideEveryRobotParts();
	}
	
	private void HideEveryRobotParts()
	{
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
		HideEveryRobotParts();
		LeftArmZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_right_arm_button_pressed()
	{
		HideEveryRobotParts();
		RightArmZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_left_leg_button_pressed()
	{
		HideEveryRobotParts();
		LeftLegZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_right_leg_button_pressed()
	{
		HideEveryRobotParts();
		RightLegZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_head_button_pressed()
	{
		HideEveryRobotParts();
		HeadZoom.Visible = true;
		this.Visible = true;
	}

	private void _on_chest_button_pressed()
	{
		HideEveryRobotParts();
		ChestZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_sword_button_pressed()
	{
		HideEveryRobotParts();
		SwordZoom.Visible = true;
		this.Visible = true;
	}
	
	private void _on_shield_button_pressed()
	{
		HideEveryRobotParts();
		ShieldZoom.Visible = true;
		this.Visible = true;
	}
}
