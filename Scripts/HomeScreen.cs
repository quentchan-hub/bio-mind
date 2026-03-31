using Godot;
using System;
public partial class HomeScreen : Control
{
	[Signal] public delegate void StartGameEventHandler();

	[Export] Brain Brain;
	[Export] EndScreen EndScreen;
	[Export] DifficultyOverlay DifficultyOverlay;
	[Export] StatsOverlay StatsOverlay;
	[Export] CollectionsOverlay CollectionsOverlay;
	
	[Export] AnimationPlayer TitleAnim;

	public override void _Ready()
	{
		DifficultyOverlay.Visible = false;
		StatsOverlay.Visible = false;
		CollectionsOverlay.Visible = false;
		TitleAnim.Play("BGTitle");
	}
	
	// Transmission vers DifficultyOverlay = GameLauncher
	private void _on_launch_btn_pressed() => DifficultyOverlay.DisplayThisOverlay();
	public void ExpertModeOpen() => DifficultyOverlay.HideBlockExpertMode();
	
	// Transmission vers StatsOverlay
	private void _on_stat_button_pressed() => StatsOverlay.Visible = true;
	
	// Transmission vers CollectionsOverlay
	// -> Affichage
	private void _on_collec_button_pressed() => CollectionsOverlay.Visible = true;
	// -> OrbCollection
	public void DisplayOrbCollection() => CollectionsOverlay.DisplayOrbCollection();
	public void DisplayOrb(int orbEarned) => CollectionsOverlay.DisplayOrb(orbEarned);
	public void OnSixOrbsCompleted() => CollectionsOverlay.OnSixOrbsCompleted();
	// -> PartCollection
	public void DisplayPartCollection() => CollectionsOverlay.DisplayPartCollection();
	public void DisplayPart(int partEarned) => CollectionsOverlay.DisplayPart(partEarned);
	public void OnEightPartsCompleted()  => CollectionsOverlay.OnEightPartsCompleted();
	// -> RobotColleciton
	public void DisplayRobotCollection() => CollectionsOverlay.DisplayRobotCollection();
	public void DisplayPose(int poseEarned) => CollectionsOverlay.DisplayPose(poseEarned);
	public void OnTwoPartsCompleted() => CollectionsOverlay.OnTwoPartsCompleted();
}
