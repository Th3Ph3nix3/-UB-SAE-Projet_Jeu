using Godot;
using System;

/// <summary>
/// Manages the game over state and related functionalities.
/// </summary>
public partial class GameOverManager : Node
{
    // #region Singleton Instance

    /// <summary>
    /// Singleton instance for global access to the GameOverManager.
    /// </summary>
    public static GameOverManager Instance { get; private set; }

    // #endregion

    // #region Private Fields

    /// <summary>
    /// Current game over menu state.
    /// </summary>
    private bool inGameOver = false;

    /// <summary>
    /// Audio player for click sounds.
    /// </summary>
    private AudioStreamPlayer clickcliksound;

    /// <summary>
    /// Audio player for background music.
    /// </summary>
    private AudioStreamPlayer musicPlayer;

    /// <summary>
    /// Audio player for death sound.
    /// </summary>
    private AudioStreamPlayer deathSound;

    /// <summary>
    /// Position in seconds to loop the death sound.
    /// </summary>
    private float loopStartPosition = 7.0f;

    /// <summary>
    /// Current pause state of the game.
    /// </summary>
    private static bool isPaused = false;

    // #endregion

    // #region Signals

    /// <summary>
    /// Signal emitted when game over menu visibility changes.
    /// </summary>
    /// <param name="inGameOver">True if game over menu is visible.</param>
    [Signal]
    public delegate void GameOverToggleEventHandler(bool inGameOver);

    // #endregion

    // #region Godot Methods

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Initializes the GameOverManager instance and audio players.
    /// </summary>
    public override void _Ready()
    {
        Instance = this;
        musicPlayer = GetNode<AudioStreamPlayer>("Music");
        deathSound = Instance.GetNode<AudioStreamPlayer>("Death");
    }

    // #endregion

    // #region Public Methods

    /// <summary>
    /// Handles the game over state, toggles pause, emits signals, and plays death sound.
    /// </summary>
    public static void GameOver()
    {
        isPaused = !isPaused;
        Instance.EmitSignal(SignalName.GameOverToggle, isPaused);
        Instance.GetTree().Paused = isPaused;
        Instance.ToggleinGameOver();
        Instance.musicPlayer.Stop();
        Instance.deathSound.Finished += Instance.AudioFinished;
        Instance.deathSound.Play();
    }

    /// <summary>
    /// Called when the death sound finishes playing. Loops the death sound from the specified position.
    /// </summary>
    private void AudioFinished()
    {
        deathSound.Play(loopStartPosition);
    }

    /// <summary>
    /// Handles the start button press event. Plays a sound and changes the scene to the game scene.
    /// </summary>
    public void _on_start_pressed()
    {
        clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
        clickcliksound.Play();
        isPaused = !isPaused;
        EmitSignal(SignalName.GameOverToggle, isPaused);
        GetTree().Paused = isPaused;
        GetTree().ReloadCurrentScene();
    }

    /// <summary>
    /// Handles the quit button press event. Plays a sound and quits the game.
    /// </summary>
    public void _on_quit_button_pressed()
    {
        clickcliksound = GetNode<AudioStreamPlayer>("ClickClickSound");
        clickcliksound.Play();
        GetTree().Quit();
    }

    /// <summary>
    /// Toggles the game over menu visibility and emits the state change signal.
    /// </summary>
    public void ToggleinGameOver()
    {
        inGameOver = !inGameOver;
        EmitSignal(SignalName.GameOverToggle, inGameOver);
    }

    // #endregion
}
