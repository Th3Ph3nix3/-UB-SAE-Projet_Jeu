using Godot;
using System;

/// <summary>
/// Manages the game's pause state and related UI/menu actions.
/// Implements singleton pattern for global access.
/// </summary>
public partial class PauseManager : Node
{
    #region Variables

    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static PauseManager Instance { get; private set; }

    // Current pause state of the game.
    private bool isPaused = false;

    // Current state of the settings menu (not used in this script but declared for future use).
    private bool inSettings = false;

    #endregion

    #region Signals

    /// <summary>
    /// Signal emitted when the game pause state is toggled.
    /// </summary>
    /// <param name="isPaused">True if the game is paused.</param>
    [Signal]
    public delegate void GamePauseToggleEventHandler(bool isPaused);

    /// <summary>
    /// Signal emitted when the settings menu visibility is toggled.
    /// </summary>
    /// <param name="inSettings">True if in settings menu.</param>
    [Signal]
    public delegate void GameSettingsToggleEventHandler(bool inSettings);

    #endregion

    #region Methods

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Initializes the singleton instance.
    /// </summary>
    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    /// Called when the pause button is pressed.
    /// Toggles the game's pause state and emits the corresponding signal.
    /// </summary>
    public void _on_pause_pressed()
    {
        isPaused = !isPaused;
        EmitSignal(SignalName.GamePauseToggle, isPaused);
        GetTree().Paused = isPaused;
    }

    /// <summary>
    /// Called when the "return to game" button is pressed.
    /// Unpauses the game and emits the corresponding signal.
    /// </summary>
    public void _on_return_game_pressed()
    {
        isPaused = !isPaused;
        EmitSignal(SignalName.GamePauseToggle, isPaused);
        GetTree().Paused = isPaused;
    }

    /// <summary>
    /// Called when the "menu" button is pressed.
    /// Unpauses the game, emits the pause signal, and changes the scene to the main menu.
    /// </summary>
    public void _on_menu_pressed()
    {
        isPaused = !isPaused;
        EmitSignal(SignalName.GamePauseToggle, isPaused);
        GetTree().Paused = isPaused;

        // Change to the main menu scene.
        GetTree().ChangeSceneToFile("res://MainMenu/MainMenu.tscn");
    }

    /// <summary>
    /// Called when the "quit" button is pressed.
    /// Quits the game application.
    /// </summary>
    public void _on_quit_button_pressed()
    {
        GetTree().Quit();
    }

    #endregion
}
