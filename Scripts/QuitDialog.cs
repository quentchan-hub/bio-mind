using Godot;
using System;

public partial class QuitDialog : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			@event.Dispose(); // évite que d'autres noeuds le traitent
			ShowQuitConfirm();
		}
	}
	
	private void ShowQuitConfirm()
	{
		Visible = true;
	}

	private void _on_ok_pressed()
	{
		GetTree().Quit();
	}
	
	private void _on_cancel_pressed()
	{
		Visible = false;
	}
	
}
