using Godot;
using System;

public partial class EndScreen : Control
{
	[Export] Label ResultLabel;
	[Export] Brain Brain;
	[Export] HBoxContainer ResultSlotContainer;
	[Export] PackedScene ResultSlotScene;
	[Export] AnimationPlayer EndScreenAnim;
	
	public override void _Ready()
	{
		Brain.DisplaySolution += OnDisplaySolution;
	}
	
	public void InstantiateResultSlots(int slots)
	{
		// RAZ préventive
		foreach (Node child in ResultSlotContainer.GetChildren())
		{
			ResultSlotContainer.RemoveChild(child);
			child.QueueFree();
		}
		
		// Instantiation des Emplacements de résultat pour l'écran final (ResultSlot)
		for (int i = 0; i < slots; i++)
		{
			var slotInstance = ResultSlotScene.Instantiate();
			slotInstance.Name = $"ResultSlot_{i}";		// <= forcer nommage en ResultSlot_1
			
			ResultSlotContainer.AddChild(slotInstance);
		}

	}
	
	public void WinCase()
	{
		ResultLabel.Text = "BRAVO !";
	}
	
	public void LoseCase()
	{
		ResultLabel.Text = "PERDU !";
	}
	
	
	private void OnDisplaySolution(Godot.Collections.Array<int> solution)
	{
		EndScreenAnim.Play("MidGlowEffect");
		var slots = ResultSlotContainer.GetChildren();
		
		for (int i = 0; i < solution.Count; i++)
		{
			var slot = slots[i];
			ResultButton button = slot.GetChild(0) as ResultButton;
			button.SetResult(solution[i]);
		}
	}
	
}
