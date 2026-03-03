using Godot;
using System;

public partial class AttemptRow : MarginContainer
{
	[Signal] public delegate void OnBoardButtonFocusedEventHandler(BoardButton button);
	[Signal] public delegate void OnFocusedRowEventHandler(AttemptRow row);
	
	[Export] HBoxContainer SlotContainer;
	[Export] PackedScene PlayerSlot;
	[Export] GridContainer HintContainer;
	[Export] PackedScene HintButton;
	[Export] AnimationPlayer RowAnim;
	[Export] ColorRect BlockTouch;
	
	public BoardButton btnFocused;
	
	public override void _Ready()
	{
		BlockTouch.Visible = true;
	}
	
	public void UpdateSlots(int slots)
	{
		// RAZ
		foreach (Node child in SlotContainer.GetChildren())
		{
			SlotContainer.RemoveChild(child);
			child.QueueFree();
		}
		
		// INSTANTIATION
		for (int i = 0 ; i < slots; i++)
		{
			var slotInstance = PlayerSlot.Instantiate();
			SlotContainer.AddChild(slotInstance);
			
			BoardButton boardButton = slotInstance.GetNode<BoardButton>("BoardButton");
			boardButton.ButtonSelected += OnBoardButtonSelected;
		}
	}
	
	private void OnBoardButtonSelected(BoardButton button)
	{
		btnFocused = button;
		EmitSignal(SignalName.OnBoardButtonFocused, button);
		EmitSignal(SignalName.OnFocusedRow, this);
	}
	
	public void UpdateHints(int hints)
	{
		// RAZ
		foreach (Node child in HintContainer.GetChildren())
		{
			HintContainer.RemoveChild(child);
			child.QueueFree();
		}
		
		// INSTANTIATION
		for (int i = 0 ; i < hints; i++)
		{
			var hintInstance = HintButton.Instantiate() as HintButton;
			HintContainer.AddChild(hintInstance);
		}
	}
	
	public void FocusFirstInRow()
	{
		var firstButton = SlotContainer.GetChild(0).GetChild(0) as BoardButton;
		btnFocused = firstButton;
		btnFocused.GrabFocus();
		btnFocused.AcceptEvent();
		GetTheRowGlow();
		OnBoardButtonSelected(btnFocused);
	}
	
	public void GetTheRowGlow()
	{
		RowAnim.Play("row_glow", -1f, 2.0f);
		BlockTouch.Visible = false;
	}
	
	public void StopAnim()
	{
		RowAnim.Stop();
		BlockTouch.Visible = true;
	}
}
