using Godot;
using System;

public partial class EndScreen : Control
{
	[Export] Label ResultLabel;
	
	public void WinCase()
	{
		ResultLabel.Text = "BRAVO !";
	}
	
	public void LoseCase()
	{
		ResultLabel.Text = "PERDU !";
	}
}
