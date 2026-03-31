using Godot;
using System;

public partial class ThumbsUpTex : TextureRect
{
	[Export] public Texture2D[] ThumbsUpTextures;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SelectThumbsUp(int index)
	{
		Texture = ThumbsUpTextures[index];
		GD.Print("test ThumbsUpTex");
	}
}
