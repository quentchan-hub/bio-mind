using Godot;
using System;

public partial class CloseDiffButton : Button
{
	[Export] Control DifficultyOverlay;
	
	private void _on_pressed()
	{
		DifficultyOverlay.Visible = false;
	}
}
