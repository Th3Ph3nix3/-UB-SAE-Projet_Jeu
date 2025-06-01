using Godot;
using System;
using System.Reflection.Emit;

public partial class Option_Frame : TextureButton
{
	#region Attributes

	/// <summary>
	/// Reference to the options class to call _close_options() method when the player click on the slot.
	/// </summary>
	private Options _options;

	private Items _item;

	#endregion

	#region Methods

	/// <summary>
	/// Custom constructor to instantiate a new Option frame from its scene.
	/// </summary>
	/// <param name="item">Item to give to the Option frame.</param>
	/// <param name="options">Refenrence of the Options class.</param>
	/// <returns>Returns a reference the the newly instantiated Option frame.</returns>
	static public Option_Frame new_OptionSlot(Items item, Options options)
	{
		Option_Frame option_frame = GD.Load<PackedScene>("res://Game/Scenes/Option_Frame.tscn").Instantiate<Option_Frame>();
		option_frame._item = item;
		option_frame._options = options;
		return option_frame;
	}

	/// <summary>
	/// Called when the node is instantiated. Set the label and description of the upgrade.
	/// </summary>
	public override void _Ready()
	{
		Godot.Label label = GetNode<Godot.Label>("Label");
		Godot.Label description = GetNode<Godot.Label>("Description");

		TextureNormal = _item.Texture;
		label.Text = "Lvl " + (_item.Level + 1).ToString();
		description.Text = _item.Upgrades[_item.Level + 1].description;
	}


	/// <summary>
	/// Called when the player clicks on the slot to upgrade the weapon or passive item.
	/// </summary>
	/// <param name="inputEvent">Input catched</param>
	public void _on_gui_input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("click"))
		{
			if (_item != null)
			{
				_item.LevelUp(); // upgrade the weapon
			}

			if (_options != null)
			{
				_options.close_options();
			}
			else
			{
				GD.PrintErr("Options reference is null in OptionSlot");
			}
		}
	}
	#endregion
}
