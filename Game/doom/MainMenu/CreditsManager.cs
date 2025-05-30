using Godot;
using System;

/// <summary>
/// Manages game credit including display modes and UI interactions
/// Implements singleton pattern for global access
/// </summary>
public partial class CreditsManager : Node
{
	#region Variables

	/// <summary>
	/// Singleton instance for global access
	/// </summary>
	public static CreditsManager Instance { get; private set; }

	// Current credit menu visibility state
	private bool inCredits = false;
	public AudioStreamPlayer clickcliksound;

	#endregion

	#region Signals

	/// <summary>
	/// Signal emitted when credit menu visibility changes
	/// </summary>
	/// <param name="inCredits">True if credit menu is visible</param>
	[Signal]
	public delegate void GameCreditsToggleEventHandler(bool inCredits);

	#endregion

	#region Methods

	/// <summary>
	/// Called when the node enters the scene tree
	/// </summary>
	public override void _Ready()
	{
		Instance = this;
		// AddItems();
	}

	/// <summary>
	/// Handles options button press to toggle credit menu
	/// </summary>
	public void _on_credits_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		ToggleCreditsMenu();
	}

	/// <summary>
	/// Handles quit button press to close credit menu
	/// </summary>
	public void _on_quit_button_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		ToggleCreditsMenu();
	}

	/// <summary>
	/// Toggles credit menu visibility and emits state change signal
	/// </summary>
	private void ToggleCreditsMenu()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		inCredits = !inCredits;
		EmitSignal(SignalName.GameCreditsToggle, inCredits);
	}

	#endregion
}
