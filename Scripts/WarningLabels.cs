using Godot;
using System;

public partial class WarningLabels : PanelContainer
{
	[Export] AnimationPlayer WarningPlayer;
	 
	public override void _Ready()
	{
		this.Visible = false;
	}
	
	public void EmptySlotWarning()
	{
		WarningPlayer.Play("EmptySlots!");
	}
	

}
