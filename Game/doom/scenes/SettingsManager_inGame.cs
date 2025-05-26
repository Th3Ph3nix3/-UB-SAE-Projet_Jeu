using Godot;
using System;

public partial class SettingsManager_inGame : Node
{
	private bool _isFullscreen = true;
	public static SettingsManager_inGame Instance { get; private set; }

	private OptionButton dropDownMenu;
	
	[Signal]
	public delegate void GameSettingsToggle_inGameEventHandler(bool inSettings);

	private bool inSettings = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		// AddItems();
	}

	public void _on_options_pressed()
	{
		inSettings = !inSettings;

		EmitSignal(SignalName.GameSettingsToggle_inGame, inSettings);
	}

	public void _on_quit_button_pressed()
	{
		inSettings = !inSettings;

		EmitSignal(SignalName.GameSettingsToggle_inGame, inSettings);
	}

	private void _on_fullscreen_pressed()
	{
		if (_isFullscreen)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			_isFullscreen = false;
		}
		else
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
			_isFullscreen = true;
		}
			
    }
}
