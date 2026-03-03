using Godot;
using System;
using System.Collections.Generic;

public partial class Test1 : Node2D
{
	private Color couleur = new Color();
	
	public override void _Input(InputEvent @event)
	{
		QueueRedraw();
	}
	
	public override void _Draw()
	{
		DrawCircle(new Vector2(10, 10), 10, couleur);
	}
	
	public void RedColor()
	{
		couleur = Colors.Red;
		QueueRedraw();
	}
	
	public void BlueColor()
	{
		couleur = Colors.Blue;
		QueueRedraw();
	}
	
}
