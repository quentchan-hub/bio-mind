using Godot;
using System;

public partial class TutoCloseButton : Button
{
	[Export] Control TutorialOverlay;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TutorialOverlay.Visible = false;
	}
	
	private void _on_pressed()
	{
		TutorialOverlay.Visible = false;
	}


}
