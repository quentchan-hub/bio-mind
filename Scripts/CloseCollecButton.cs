using Godot;
using System;

public partial class CloseCollecButton : Button
{
	[Export] Control CollectionsOverlay;
	
	private void _on_pressed()
	{
		CollectionsOverlay.Visible = false;
	}

}
