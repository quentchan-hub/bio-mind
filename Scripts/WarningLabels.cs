using Godot;
using System;

public partial class WarningLabels : PanelContainer
{
	[Export] AnimationPlayer WarningPlayer;
	[Export] Label WarningEmptySlots;
	[Export] Label WarningNoDouble;
	 
	public override void _Ready()
	{
		this.Visible = false;
	}
	
	public void EmptySlotWarning()
	{
		// Affiche uniquement le message "Emplacement(s) vide(s)"
		WarningEmptySlots.Visible = true;
		WarningNoDouble.Visible = false;
		WarningPlayer.Play("EmptySlots!");
	}
	
	public void NoDoubleColorWarning()
	{
		// Affiche uniquement le message "Couleurs en double"
		WarningEmptySlots.Visible = false;
		WarningNoDouble.Visible = true;
		WarningPlayer.Play("NoDouble!");
	}
	

}
