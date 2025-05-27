using Godot;
using System;

/// <summary>
/// Handles in-game screen resolution selection and window size configuration.
/// Provides button callbacks for common resolution presets.
/// </summary>
public partial class Resolution_button_inGame : VBoxContainer
{
    #region Variables

    // TODO: Implement fullscreen toggle functionality if needed
    private bool _isFullscreen = true;

    #endregion

    #region Methods

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        // Initialization logic can be added here if necessary
    }

    /// <summary>
    /// Sets the window resolution to 1024x546.
    /// </summary>
    public void _on_1024x546_pressed()
    {
        var window = GetWindow();
        window.Size = new Vector2I(1024, 546);
    }

    /// <summary>
    /// Sets the window resolution to 1280x720 (HD).
    /// </summary>
    public void _on_1280x720_pressed()
    {
        var window = GetWindow();
        window.Size = new Vector2I(1280, 720);
    }

    /// <summary>
    /// Sets the window resolution to 1600x900 (HD+).
    /// </summary>
    public void _on_1600x900_pressed()
    {
        var window = GetWindow();
        window.Size = new Vector2I(1600, 900);
    }

    /// <summary>
    /// Sets the window resolution to 1920x1080 (Full HD).
    /// </summary>
    public void _on_1920x1080_pressed()
    {
        var window = GetWindow();
        window.Size = new Vector2I(1920, 1080);
    }

    /// <summary>
    /// Sets the window resolution to 1920x1200 (WUXGA).
    /// </summary>
    public void _on_1920x1200_pressed()
    {
        var window = GetWindow();
        window.Size = new Vector2I(1920, 1200);
    }

    #endregion
}
