using Godot;
using System;

public partial class CloseStatsButton : Button
{
	[Export] Control StatsOverlay;
	
	private void _on_pressed()
	{
		StatsOverlay.Visible = false;
	}
}
