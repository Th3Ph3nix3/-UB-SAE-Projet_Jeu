using Godot;
using System;

/// <summary>
/// Manages game settings including display modes and UI interactions
/// Implements singleton pattern for global access
/// </summary>
public partial class SettingsManager : Node
{
	#region Variables

	/// <summary>
	/// Singleton instance for global access
	/// </summary>
	public static SettingsManager Instance { get; private set; }

	// Window display state
	private bool _isFullscreen = true;

	private OptionButton dropDownMenu;

	// Current settings menu visibility state
	private bool inSettings = false;
	public AudioStreamPlayer clickcliksound;

	#endregion

	#region Signals

	/// <summary>
	/// Signal emitted when settings menu visibility changes
	/// </summary>
	/// <param name="inSettings">True if settings menu is visible</param>
	[Signal]
	public delegate void GameSettingsToggleEventHandler(bool inSettings);

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
	/// Handles options button press to toggle settings menu
	/// </summary>
	public void _on_options_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		ToggleSettingsMenu();
	}

	/// <summary>
	/// Handles quit button press to close settings menu
	/// </summary>
	public void _on_quit_button_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		ToggleSettingsMenu();
	}

	/// <summary>
	/// Toggles window mode between fullscreen and windowed
	/// </summary>
	private void _on_fullscreen_pressed()
	{
		clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
		clickcliksound.Play();
		_isFullscreen = !_isFullscreen;
		var mode = _isFullscreen 
			? DisplayServer.WindowMode.Fullscreen 
			: DisplayServer.WindowMode.Windowed;
		
		DisplayServer.WindowSetMode(mode);
	}

	/// <summary>
	/// Toggles settings menu visibility and emits state change signal
	/// </summary>
	private void ToggleSettingsMenu()
	{
		inSettings = !inSettings;
		EmitSignal(SignalName.GameSettingsToggle, inSettings);
	}

	#endregion
}
