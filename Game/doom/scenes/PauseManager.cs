using Godot;
using System;

public partial class PauseManager : Node
{
	public static PauseManager Instance { get; private set; }

	[Signal]
	public delegate void GamePauseToggleEventHandler(bool isPaused);

	private bool isPaused = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	public void _on_pause_pressed()
	{
		isPaused = !isPaused;

		EmitSignal(SignalName.GamePauseToggle, isPaused);
		GetTree().Paused = isPaused;
	}

	public void _on_return_game_pressed()
	{
		isPaused = !isPaused;

		EmitSignal(SignalName.GamePauseToggle, isPaused);
		GetTree().Paused = isPaused;
	}

	public void _on_options_pressed()
	{
		// isPaused = !isPaused;

		// EmitSignal(SignalName.GamePauseToggle, isPaused);
		// GetTree().Paused = isPaused;

		GD.Print("option pressed");
	}

	public void _on_menu_pressed()
	{
		isPaused = !isPaused;

		EmitSignal(SignalName.GamePauseToggle, isPaused);
		GetTree().Paused = isPaused;

		GetTree().ChangeSceneToFile("res://MainMenu/MainMenu.tscn");
	}

	public void _on_quit_button_pressed()
	{
		GetTree().Quit();
	}
}
