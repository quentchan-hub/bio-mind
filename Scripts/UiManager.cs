using Godot;
using System;

public partial class UiManager : Control
{
	[Export] HomeScreen HomeScreen;
	[Export] GameScreen GameScreen;
	[Export] EndScreen EndScreen;
	[Export] Brain Brain;
	
	public override void _Ready()
	{
		DisplayHomeScreen();
		Brain.OnGameOver += DisplayEndScreen;
	}

	private void _on_play_btn_pressed()
	{
		DisplayGameScreen();
		//GameScreen.FirstButtonFocus();
	}
		
	private void _on_play_again_pressed()
	{
		DisplayGameScreen();
	}
	
	private void _on_back_home_pressed()
	{
		DisplayHomeScreen();
	}
	
	private void _on_home_button_pressed()
	{
		DisplayHomeScreen();
	}

	
	private void DisplayHomeScreen()
	{
		EndScreen.Visible = false;
		GameScreen.Visible = false;
		HomeScreen.Visible = true;
	}
	
	private void DisplayGameScreen()
	{
		HomeScreen.Visible = false;
		GameScreen.Visible = true;
		EndScreen.Visible = false;
	}
	
	private void DisplayEndScreen(bool victory)
	{
		HomeScreen.Visible = false;
		GameScreen.Visible = false;
		EndScreen.Visible = true;
		
		if (victory == true)
		{
			EndScreen.WinCase();
		}
		
		if (victory == false)
		{
			EndScreen.LoseCase();
		}
	}
}
