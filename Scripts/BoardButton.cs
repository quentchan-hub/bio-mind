using Godot;
using System;

public partial class BoardButton : Button
{
	[Signal] public delegate void ButtonSelectedEventHandler(BoardButton button);
	
	[Export] public Texture2D[] HeadTextures;
	
	public enum HeadColor
	{
		Black,		// index 0
		Blue,		// index 1
		Purple,		// index 2 etc. 
		Red,
		White,
		Yellow
	}
	
	public override void _Ready()
	{
		ClearIcon();
	}

	public void SetHead(int index)
	{
		if (index >= 0 && index < HeadTextures.Length)
			Icon = HeadTextures[index];
	}
	
	private void _on_pressed()
	{
		GrabFocus();
		EmitSignal(SignalName.ButtonSelected, this);
	}
	
	public void ClearIcon()
	{
		Icon = null;
	}
}
