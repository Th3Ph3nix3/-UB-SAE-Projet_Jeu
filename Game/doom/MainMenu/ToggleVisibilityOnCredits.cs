using Godot;
using System;

/// <summary>
/// Toggles the visibility of this CanvasLayer based on the credits menu state.
/// </summary>
public partial class ToggleVisibilityOnCredits : CanvasLayer
{
    #region Variables

    /// <summary>
    /// Determines if this node should be visible when the credits menu is open.
    /// Set in the inspector.
    /// </summary>
    [Export]
    bool visibleOnCredits = true;

    #endregion

    #region Methods

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Subscribes to the credits menu toggle event and sets initial visibility.
    /// </summary>
    public override void _Ready()
    {
        // Subscribe to the credits menu visibility toggle signal
        CreditsManager.Instance.GameCreditsToggle += ToggleVisibility;

        // Hide the node initially if it should only be shown in credits
        if (!visibleOnCredits) return;

        Hide();
    }

    /// <summary>
    /// Callback to toggle this node's visibility based on the credits menu state.
    /// </summary>
    /// <param name="inCredits">True if the credits menu is open.</param>
    private void ToggleVisibility(bool inCredits)
    {
        if (visibleOnCredits == inCredits)
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
