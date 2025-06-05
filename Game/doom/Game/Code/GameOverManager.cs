using Godot;
using System;

public partial class GameOverManager : Node
{
	// Singleton instance for global access.
    public static GameOverManager Instance { get; private set; }
	
	// Current settings menu state
	private bool inGameOver = false;
	private AudioStreamPlayer clickcliksound;
	// Current pause state of the game.
	private static bool isPaused = false;

	#region Signals

	/// <summary>
	/// Signal emitted when settings menu visibility changes
	/// </summary>
	/// <param name="inGameOver">True if settings menu is visible</param>
	[Signal]
    public delegate void GameOverToggleEventHandler(bool inGameOver);

	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	public static void GameOver()
	{
		isPaused = !isPaused;
		Instance.EmitSignal(SignalName.GameOverToggle, isPaused);
		Instance.GetTree().Paused = isPaused;
		Instance.ToggleinGameOver();
	}
	
	public void _on_start_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		isPaused = !isPaused;
		EmitSignal(SignalName.GameOverToggle, isPaused);
		GetTree().Paused = isPaused;
		GetTree().ChangeSceneToFile("res://Game/Scenes/Game_Test.tscn");
	}
	
	/// <summary>
	/// Handles quit button press to close settings menu
	/// </summary>
	public void _on_quit_button_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		GetTree().Quit();
	}

	/// <summary>
	/// Toggles settings menu visibility and emits state change signal
	/// </summary>
	public void ToggleinGameOver()
	{
		inGameOver = !inGameOver;
		EmitSignal(SignalName.GameOverToggle, inGameOver);
	}
}
