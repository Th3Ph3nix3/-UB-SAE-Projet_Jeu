using Godot;
using System;
using System.Reflection.Emit;

public partial class UpgradeItem_Frame : TextureButton
{
	#region Attributes

	/// <summary>
	/// Item to give to the upgrade item frame.
	/// </summary>
	private Items _item;

	#endregion

	#region Methods

	/// <summary>
	/// Custom constructor to instantiate a new Option frame from its scene.
	/// </summary>
	/// <param name="item">Item to give to the Option frame.</param>
	/// <param name="options">Refenrence of the Options class.</param>
	/// <returns>Returns a reference the the newly instantiated Option frame.</returns>
	static public UpgradeItem_Frame new_UpgradeItem_Frame(Items item)
	{
		UpgradeItem_Frame upgrade_frame = GD.Load<PackedScene>("res://Game/Scenes/UI/Upgrade_UI/UpgradeItem_Frame.tscn").Instantiate<UpgradeItem_Frame>();
		upgrade_frame._item = item;
		return upgrade_frame;
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
				UI.LevelUp_Panel.Close(); // close the options panel
			}
			else
			{
				GD.PrintErr("Upgrade_Frame : Item cannot be null.");
			}
		}
	}

	#endregion

}
