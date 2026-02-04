using Godot;
using System;

public partial class GlowGreenEffect : TextureRect
{
	[Export] AnimationPlayer coffreAnim; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		coffreAnim.Play("GlowGreen");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
