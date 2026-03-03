using Godot;
using System;

public partial class ResultButton : Button
{
	[Export] public Texture2D[] ResultTextures;
	
	public override void _Ready()
	{
		ClearIcon();
	}
	
	public void SetResult(int index)
	{
		if (index >= 0 && index < ResultTextures.Length)
			Icon = ResultTextures[index];
	}
	
	public void ClearIcon()
	{
		Icon = null;
	}
}
