using Godot;
using System;

public partial class ToggleVisibilityOnSettings_inGame : CanvasLayer
{
	[Export] bool visibleOnSettings_inGame = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SettingsManager_inGame.Instance.GameSettingsToggle_inGame += ToggleVisibility;

		if (!visibleOnSettings_inGame) return;

		Hide();
	}

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
}
