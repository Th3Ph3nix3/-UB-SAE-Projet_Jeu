using Godot;
using System;

/// <summary>
/// Toggles the visibility of this CanvasLayer based on the game's pause state.
/// </summary>
public partial class ToggleVisibilityOnPause : CanvasLayer
{
    #region Variables

    /// <summary>
    /// Determines if this node should be visible when the game is paused.
    /// Set in the inspector.
    /// </summary>
    [Export]
    bool visibleOnPause = true;

    #endregion

    #region Methods

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Subscribes to the pause state toggle event and sets initial visibility.
    /// </summary>
    public override void _Ready()
    {
        // Subscribe to the pause state toggle signal
        PauseManager.Instance.GamePauseToggle += ToggleVisibility;

        // Hide the node initially if it should only be shown when paused
        if (!visibleOnPause) return;

        Hide();
    }

    /// <summary>
    /// Callback to toggle this node's visibility based on the game's pause state.
    /// </summary>
    /// <param name="isPaused">True if the game is paused.</param>
    private void ToggleVisibility(bool isPaused)
    {
        if (visibleOnPause == isPaused)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    #endregion
}
