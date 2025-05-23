using Godot;
using System;

public partial class SettingsManager : Node
{
	public static SettingsManager Instance { get; private set; }

	private OptionButton dropDownMenu;
	
	[Signal]
	public delegate void GameSettingsToggleEventHandler(bool inSettings);

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

		EmitSignal(SignalName.GameSettingsToggle, inSettings);
	}

	public void _on_quit_button_pressed()
	{
		inSettings = !inSettings;

		EmitSignal(SignalName.GameSettingsToggle, inSettings);
	}

	// private void AddItems()
	// {
	// 	dropDownMenu.AddItem("1024x546");
	// 	dropDownMenu.AddItem("1280x720");
	// 	dropDownMenu.AddItem("1600x900");
	// 	dropDownMenu.AddItem("1920x1080");
	// }

	// public void _on_OptionButton_item_selected(long index)
	// {
	// 	var currentSelected = index;
	// 	// Récupérer la fenêtre courante
	// 	var window = GetWindow();

	// 	if (currentSelected == 0)
	// 		window.Size = new Vector2I(1024, 546);
	// 	if (currentSelected == 1)
	// 		window.Size = new Vector2I(1280, 720);
	// 	if (currentSelected == 2)
	// 		window.Size = new Vector2I(1600, 900);
	// 	if (currentSelected == 3)
	// 		window.Size = new Vector2I(1920, 1080);
	// }
}
