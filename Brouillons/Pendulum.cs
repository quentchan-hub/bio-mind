using Godot;
using System;


public partial class Pendulum : Node2D
{
	[Export] private float delay;
	[Export] Sprite2D circle;
	
	public override void _Ready()
	{
		GetTree().CreateTimer(delay).Timeout += () => 
		{
			Tween tween = GetTree().CreateTween().SetLoops().SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
				tween.TweenProperty(this, "rotation", -Mathf.Pi/2.0, 2.0f);
				tween.TweenProperty(this, "rotation", Mathf.Pi/2.0, 2.0f);
		};
		
		
	}
}
