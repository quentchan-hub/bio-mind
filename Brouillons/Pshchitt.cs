using Godot;
using System;

public partial class Pshchitt : Node2D
{
	[Export] Test1 Test1;
	
	public override void _Ready()
	{
		Test1.RedColor();
	}
	
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("ui_accept"))
		{
			Test1.BlueColor();
		}
	}
}
