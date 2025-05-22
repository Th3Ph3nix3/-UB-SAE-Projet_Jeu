using Godot;
using System;

public partial class ToggleVisibilityOnSettings : CanvasLayer
{
	[Export] bool visibleOnSettings = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SettingsManager.Instance.GameSettingsToggle += ToggleVisibility;

		if (!visibleOnSettings) return;

		Hide();
	}

	private void ToggleVisibility(bool inSettings)
	{
		if (visibleOnSettings == inSettings)
		{
			Show();
		}
		else
		{
			Hide();
		}
	}
}
