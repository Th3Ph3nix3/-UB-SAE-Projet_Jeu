using Godot;
using System;

/// <summary>
/// Manages in-game settings functionality including display modes and UI interactions
/// Implements singleton pattern for global access
/// </summary>
public partial class SettingsManager_inGame : Node
{
    #region Variables

    // Singleton instance for global access.
    public static SettingsManager_inGame Instance { get; private set; }

    // Window state
    private bool _isFullscreen = true;

    // Reference to the dropdown menu (to be assigned in the scene)
    private OptionButton dropDownMenu;

    // Current settings menu state
    private bool inSettings = false;
    private AudioStreamPlayer clickcliksound;
    #endregion

    #region Signals

    /// <summary>
    /// Signal emitted when settings menu visibility changes
    /// </summary>
    /// <param name="inSettings">True if settings menu is visible</param>
    [Signal]
    public delegate void GameSettingsToggle_inGameEventHandler(bool inSettings);

    #endregion

    #region Methods

    /// <summary>
    /// Called when the node enters the scene tree
    /// Initializes singleton instance
    /// </summary>
    public override void _Ready()
    {
        Instance = this;
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
        EmitSignal(SignalName.GameSettingsToggle_inGame, inSettings);
    }

    #endregion
}