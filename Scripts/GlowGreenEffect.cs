using Godot;
using System;

public partial class GlowGreenEffect : TextureRect
{
	[Export] AnimationPlayer CoffreAnim;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	private void _on_play_btn_pressed()
	{
		CoffreAnim.Play("GlowGreen");
	}
}
