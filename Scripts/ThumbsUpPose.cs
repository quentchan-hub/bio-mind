using Godot;
using System;

public partial class ThumbsUpPose : Control
{
	[Export] ThumbsUpTex ThumbsUpTex;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	public void ThumbsUpRandomSelector()
	{
		int randomThumbsUp = (int)(GD.Randi() % 3);
		ThumbsUpTex.SelectThumbsUp(randomThumbsUp);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
