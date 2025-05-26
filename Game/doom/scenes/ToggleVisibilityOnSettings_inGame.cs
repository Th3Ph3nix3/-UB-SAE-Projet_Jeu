using Godot;
using System;

/// <summary>
/// Toggles the visibility of this CanvasLayer based on the in-game settings menu state.
/// </summary>
public partial class ToggleVisibilityOnSettings_inGame : CanvasLayer
{
    #region Variables

    /// <summary>
    /// Determines if this node should be visible when in settings.
    /// Set in the inspector.
    /// </summary>
    [Export]
    bool visibleOnSettings_inGame = true;

    #endregion

    #region Methods

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Subscribes to the settings menu toggle event and sets initial visibility.
    /// </summary>
    public override void _Ready()
    {
        // Subscribe to the settings menu visibility toggle signal
        SettingsManager_inGame.Instance.GameSettingsToggle_inGame += ToggleVisibility;

        // Hide the node initially if it should only be shown in settings
        if (!visibleOnSettings_inGame) return;

        Hide();
    }

    /// <summary>
    /// Callback to toggle this node's visibility based on the settings menu state.
    /// </summary>
    /// <param name="inSettings">True if the settings menu is open.</param>
    private void ToggleVisibility(bool inSettings)
    {
        if (visibleOnSettings_inGame == inSettings)
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
