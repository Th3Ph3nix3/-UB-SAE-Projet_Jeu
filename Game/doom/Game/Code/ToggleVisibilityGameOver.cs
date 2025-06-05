using Godot;
using System;

/// <summary>
/// Manages the visibility of UI elements based on the game over state.
/// </summary>
public partial class ToggleVisibilityGameOver : CanvasLayer
{
    // #region Exported Variables

    /// <summary>
    /// Determines if this node should be visible when in Game Over.
    /// Set in the inspector.
    /// </summary>
    [Export]
    bool visibleGameOver = true;

    // #endregion

    // #region Godot Methods

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Subscribes to the game over toggle signal and initializes visibility.
    /// </summary>
    public override void _Ready()
    {
        // Subscribe to the game over visibility toggle signal
        GameOverManager.Instance.GameOverToggle += ToggleVisibility;

        // Hide the node initially if it should only be shown in Game Over
        if (!visibleGameOver) return;

        Hide();
    }

    // #endregion

    // #region Private Methods

    /// <summary>
    /// Callback to toggle this node's visibility based on the game over state.
    /// </summary>
    /// <param name="inGameOver">True if the game over menu is open.</param>
    private void ToggleVisibility(bool inGameOver)
    {
        if (visibleGameOver == inGameOver)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    // #endregion
}
