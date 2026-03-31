using Godot;
using System;

public partial class ExpertMode : PanelContainer
{
	[Export] Label LevelDescriptLabel;
	[Export] Label LevelDescriptLabel2;
	[Export] Label LevelDescriptLabel3;
	[Export] Label LevelDescriptLabel4;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void LevelDescription(int slots, int colors, int maxTry, bool colorRepeat)
	{
		string slt = slots.ToString();
		string col = colors.ToString();
		string mTry = maxTry.ToString(); 
		
		LevelDescriptLabel.Text = $"{slt} couleurs à trouver.";
		LevelDescriptLabel4.Text = $"{col} couleurs à disposition.";
		LevelDescriptLabel2.Text = $"{mTry} essais possible.";
		
		string cRep = colorRepeat ? 
		LevelDescriptLabel3.Text = "Répétition de couleurs possible."
		: LevelDescriptLabel3.Text = "Pas de répétition de couleur possible.";
	}
}
