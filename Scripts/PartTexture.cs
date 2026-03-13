using Godot;
using System;

public partial class PartTexture : TextureRect
{
	[Export] public Texture2D[] PartTextures;
	
	public enum Part
	{
		LeftArm,
		RightArm,
		LeftLeg,
		RightLeg,
		Head,
		Chest,
		Sword,
		Shield
	}
	
	public void Select( int index ) 		// index envoyé par PartRandomSelector dans EndScreen
	{
		Texture = PartTextures[index];
	}
	
}
