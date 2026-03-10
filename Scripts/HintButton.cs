using Godot;
using System;

public partial class HintButton : ColorRect
{
	private bool _isHintGiven = false;
	private Color hintColor = new Color();
	 
	public override void _Ready()
	{
		QueueRedraw();
	}
	
	public override void _Draw()
	{
		DrawCircle(new Vector2(0,0), 10, Colors.DarkSlateBlue);
		DrawCircle(new Vector2(0,0), 8, Colors.Black);
		
		if (_isHintGiven == true)
		{
			DrawCircle(new Vector2(0,0), 10, hintColor);
			
		}		
	}
	
	public void RedHint()
	{
		_isHintGiven = true;
		hintColor = Colors.Red;
		QueueRedraw();
	}
	
	public void WhiteHint()
	{
		_isHintGiven = true;
		hintColor = Colors.GhostWhite;
		QueueRedraw();
	}
}
