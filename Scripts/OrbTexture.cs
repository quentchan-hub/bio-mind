using Godot;
using System;

public partial class OrbTexture : TextureRect
{
	[Export] public Texture2D[] OrbTextures;
	
	public enum OrbColor
	{
		black,
		blue,
		purple,
		red,
		white,
		yellow
	}
	
	public void Select(int index)
	{
		Texture = OrbTextures[index];
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
