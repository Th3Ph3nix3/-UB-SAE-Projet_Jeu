using Godot;
using System;

public partial class ToggleVisibilityGameOver : CanvasLayer
{
	/// <summary>
	/// Determines if this node should be visible when in Game Over.
	/// Set in the inspector.
	/// </summary>
	[Export]
	bool visibleGameOver = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Subscribe to the settings menu visibility toggle signal
		GameOverManager.Instance.GameOverToggle += ToggleVisibility;

		// Hide the node initially if it should only be shown in Game Over
        if (!visibleGameOver) return;

		Hide();
	}
	
	/// <summary>
    /// Callback to toggle this node's visibility based on Game Over.
    /// </summary>
    /// <param name="inGameOver">True if the settings menu is open.</param>
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
}
