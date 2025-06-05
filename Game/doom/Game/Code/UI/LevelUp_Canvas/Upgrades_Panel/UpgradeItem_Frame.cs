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

	/// <summary>
	/// Flag to know if a item upgrade is a new item or not.
	/// </summary>
	private bool _newItem = false;

	#endregion

	#region event

	/// <summary>
	/// Event raised when the button as been clicked.
	/// </summary>
	public event EventHandler<ItemClickedEventArgs> ItemClickedEvent;

	/// <summary>
	/// Class used to pass the ItemClicked event data.
	/// </summary>
	public class ItemClickedEventArgs : EventArgs
	{
		public Items Item { get; }
		public bool IsNew { get; }
		public ItemClickedEventArgs(Items item, bool isNew) { Item = item; IsNew = isNew; }
	}

	#endregion

	#region Methods

	/// <summary>
	/// Custom constructor to instantiate a new Option frame from its scene.
	/// </summary>
	/// <param name="item">Item to give to the Option frame.</param>
	/// <param name="newItem">True if the item is a new item, false otherwise.</param>
	/// <returns>Returns a reference the the newly instantiated UpgradeItem frame.</returns>
	static public UpgradeItem_Frame new_UpgradeItem_Frame(Items item)
	{
		UpgradeItem_Frame upgrade_frame = GD.Load<PackedScene>("res://Game/Scenes/UI/LevelUp_Canvas/UpgradeItem_Frame.tscn").Instantiate<UpgradeItem_Frame>();
		upgrade_frame._item = item;
		upgrade_frame._newItem = item.Holder == null;
		return upgrade_frame;
	}

	/// <summary>
	/// Called when the node is instantiated. Set the label and description of the upgrade.
	/// </summary>
	public override void _Ready()
	{
		Godot.Label label = GetNode<Godot.Label>("NinePatchRect/Label");
		Godot.Label description = GetNode<Godot.Label>("NinePatchRect/Description");

		TextureNormal = _item.Texture;
		label.Text = _newItem ? "New !" : "Lvl " + (_item.Level + 1).ToString();
		description.Text = _item.Upgrades[_item.Level + (_newItem ? 0 : 1)].description;
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
				ItemClickedEvent?.Invoke(this, new ItemClickedEventArgs(_item, _newItem));
			}
			else
			{
				GD.PrintErr("Upgrade_Frame : Item cannot be null.");
			}
		}
	}

	#endregion

}
