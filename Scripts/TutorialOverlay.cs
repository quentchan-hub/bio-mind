using Godot;
using System;
using System.Threading.Tasks;

public partial class TutorialOverlay : Control
{
	[Export] AnimationPlayer tutoAnim;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private async void _on_help_button_pressed()
	{
		await PlayAnim3Times(tutoAnim, "HintExplanation");
	}
	
	
	public async Task PlayAnim3Times(AnimationPlayer anim, string name)
	{
		for (int i = 0; i < 3; i++)
		{
			anim.Play(name);
			await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);
		}
	}

}
