using Godot;
using System;

public partial class StatsOverlay : Control
{
	[Export] ConfirmationDialog ConfirmResetStat;
	[Export] Brain Brain;
	[Export] GameScreen GameScreen;

	// Labels Facile
	[Export] Label EasyValueLabel;
	[Export] Label WinEasyValueLabel;

	// Labels Difficile
	[Export] Label HardValueLabel;
	[Export] Label HardWinValueLabel;

	// Labels Expert
	[Export] Label ExpertValueLabel;
	[Export] Label ExpertWinValueLabel;

	// Labels Généraux
	[Export] Label GeneralValueLabel;
	[Export] Label WinGeneralValueLabel;

	// Labels Meilleurs Temps
	[Export] Label EasyChronoValue;
	[Export] Label HardChronoValue;
	[Export] Label ExpertChronoValue;

	// Compteurs en mémoire
	private int easyGame   = 0;
	private int mediumGame = 0;
	private int hardGame   = 0;
	private int expertGame = 0;

	private int easyWin   = 0;
	private int mediumWin = 0;
	private int hardWin   = 0;
	private int expertWin = 0;

	// Meilleurs temps (en secondes, 0 = pas encore de record)
	private int easyBestTime   = 0;
	private int hardBestTime   = 0;
	private int expertBestTime = 0;

	private ConfigFile _config = new ConfigFile();

	public override void _Ready()
	{
		Brain.OnAccountingGamePlayed += OnAccountingGamePlayed;
		GameScreen.ChronoTime += OnChronoTime; 
		Visible = false;
		ConfirmResetStat.Visible = false;
		LoadData();
	}

	// ============================================================
	// COMPTABILISATION
	// ============================================================

	private void OnAccountingGamePlayed(int difficulty, bool victory)
	{
		switch (difficulty)
		{
			case 1:
				easyGame++;
				if (victory) easyWin++;
				_config.SetValue("Stats", "EasyGamePlayed", easyGame);
				_config.SetValue("Stats", "EasyGameWon",   easyWin);
				break;

			case 2:
				mediumGame++;
				if (victory) mediumWin++;
				_config.SetValue("Stats", "MediumGamePlayed", mediumGame);
				_config.SetValue("Stats", "MediumGameWon",   mediumWin);
				break;

			case 3:
				hardGame++;
				if (victory) hardWin++;
				_config.SetValue("Stats", "HardGamePlayed", hardGame);
				_config.SetValue("Stats", "HardGameWon",   hardWin);
				break;

			case 4:
				expertGame++;
				if (victory) expertWin++;
				_config.SetValue("Stats", "ExpertGamePlayed", expertGame);
				_config.SetValue("Stats", "ExpertGameWon",   expertWin);
				break;
		}

		_config.Save("user://stats.cfg");
		RefreshLabels();
	}

	// ============================================================
	// CHRONO
	// ============================================================

	private void OnChronoTime(int chronoElapsed, int difficulty)
	{
		bool isNewRecord = false;

		switch (difficulty)
		{
			case 1:
				if (easyBestTime == 0 || chronoElapsed < easyBestTime)
				{
					easyBestTime = chronoElapsed;
					_config.SetValue("Stats", "EasyBestTime", easyBestTime);
					isNewRecord = true;
				}
				break;
			case 3:
				if (hardBestTime == 0 || chronoElapsed < hardBestTime)
				{
					hardBestTime = chronoElapsed;
					_config.SetValue("Stats", "HardBestTime", hardBestTime);
					isNewRecord = true;
				}
				break;
			case 4:
				if (expertBestTime == 0 || chronoElapsed < expertBestTime)
				{
					expertBestTime = chronoElapsed;
					_config.SetValue("Stats", "ExpertBestTime", expertBestTime);
					isNewRecord = true;
				}
				break;
		}

		if (isNewRecord)
		{
			_config.Save("user://stats.cfg");
			GD.Print($"Nouveau record ! difficulté {difficulty} : {chronoElapsed}s");
		}

		RefreshChronoLabels();
	}

	// -v- totalSeconds = GameScreen.elapsed -v-
	private string FormatTime(int totalSeconds) 
	{
		if (totalSeconds == 0) return "--:--";
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;
		GD.Print($"test formatage temps retenu {minutes:00}:{seconds:00}");
		return $"{minutes:00} : {seconds:00}";
		
	}

	// ============================================================
	// AFFICHAGE
	// ============================================================

	private void RefreshLabels()
	{
		if (EasyValueLabel   != null) EasyValueLabel.Text   = $"{easyGame}";
		if (WinEasyValueLabel != null)
		{
			int easyPercent = easyGame > 0 ? (int)((float)easyWin / easyGame * 100) : 0;
			WinEasyValueLabel.Text = $"{easyPercent}%";
		}

		if (HardValueLabel   != null) HardValueLabel.Text   = $"{hardGame}";
		if (HardWinValueLabel != null)
		{
			int hardPercent = hardGame > 0 ? (int)((float)hardWin / hardGame * 100) : 0;
			HardWinValueLabel.Text = $"{hardPercent}%";
		}

		if (ExpertValueLabel   != null) ExpertValueLabel.Text   = $"{expertGame}";
		if (ExpertWinValueLabel != null)
		{
			int expertPercent = expertGame > 0 ? (int)((float)expertWin / expertGame * 100) : 0;
			ExpertWinValueLabel.Text = $"{expertPercent}%";
		}

		int totalGame = easyGame + mediumGame + hardGame + expertGame;
		int totalWin  = easyWin  + mediumWin  + hardWin  + expertWin;
		if (GeneralValueLabel    != null) GeneralValueLabel.Text    = $"{totalGame}";
		if (WinGeneralValueLabel != null)
		{
			int generalPercent = totalGame > 0 ? (int)((float)totalWin / totalGame * 100) : 0;
			WinGeneralValueLabel.Text = $"{generalPercent}%";
		}

		RefreshChronoLabels();
	}

	private void RefreshChronoLabels()
	{
		EasyChronoValue.Text   = FormatTime(easyBestTime);
		GD.Print("teste le passage au label : " +  FormatTime(easyBestTime));
		HardChronoValue.Text   = FormatTime(hardBestTime);
		ExpertChronoValue.Text = FormatTime(expertBestTime);
	}

	// ============================================================
	// RESET
	// ============================================================

	private void _on_reset_stat_button_pressed() => ConfirmResetStat.Visible = true;

	private void _on_confirmation_dialog_confirmed()
	{
		DirAccess dir = DirAccess.Open("user://");
		dir.Remove("stats.cfg");

		_config    = new ConfigFile();
		easyGame   = mediumGame = hardGame = expertGame = 0;
		easyWin    = mediumWin  = hardWin  = expertWin  = 0;
		easyBestTime = hardBestTime = expertBestTime = 0;

		RefreshLabels();
		ConfirmResetStat.Visible = false;
	}

	public void ResetData() => _on_confirmation_dialog_confirmed();

	// ============================================================
	// CHARGEMENT
	// ============================================================

	private void LoadData()
	{
		Error err = _config.Load("user://stats.cfg");
		if (err != Error.Ok) return;

		easyGame   = (int)_config.GetValue("Stats", "EasyGamePlayed",   0);
		easyWin    = (int)_config.GetValue("Stats", "EasyGameWon",      0);
		mediumGame = (int)_config.GetValue("Stats", "MediumGamePlayed", 0);
		mediumWin  = (int)_config.GetValue("Stats", "MediumGameWon",    0);
		hardGame   = (int)_config.GetValue("Stats", "HardGamePlayed",   0);
		hardWin    = (int)_config.GetValue("Stats", "HardGameWon",      0);
		expertGame = (int)_config.GetValue("Stats", "ExpertGamePlayed", 0);
		expertWin  = (int)_config.GetValue("Stats", "ExpertGameWon",    0);

		easyBestTime   = (int)_config.GetValue("Stats", "EasyBestTime",   0);
		hardBestTime   = (int)_config.GetValue("Stats", "HardBestTime",   0);
		expertBestTime = (int)_config.GetValue("Stats", "ExpertBestTime", 0);

		RefreshLabels();
	}
}
