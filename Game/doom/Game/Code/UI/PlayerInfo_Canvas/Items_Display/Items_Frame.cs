using Godot;
using System;
using System.Net.WebSockets;

public partial class Items_Frame : PanelContainer
{

	#region attributes

	/// <summary>
	/// Texture of the weapon to be displayed in this slot.
	/// </summary>
	private TextureRect _tex;

	/// <summary>
	/// Label displaying it's current level.
	/// </summary>
	private Label _label;

	/// <summary>
	/// Item to be displayed in this slot.
	/// </summary>
	private Items _item;

	#endregion

	#region properties

	public Items Item { get => _item; }

	#endregion

	#region methods

	/// <summary>
	/// Custom constructor to instantiate a new Item frame from its scene.
	/// </summary>
	/// <param name="item">Item to give to the Item frame.</param>
	/// <returns>Returns a reference the the newly instantiated Item frame.</returns>
	static public Items_Frame new_Items_Frame(Items item)
	{
		Items_Frame items_frame = GD.Load<PackedScene>("res://Game/Scenes/UI/Items_Display/Items_Frame.tscn").Instantiate<Items_Frame>();
		items_frame._item = item;
		return items_frame;
	}

	/// <summary>
	/// Called when the node is added to the scene. Load the texture of the item on the scene.
	/// </summary>
	public override void _Ready()
	{
		_tex = GetNode<TextureRect>("TextureRect");
		_label = GetNode<Label>("Label");

		_tex.Texture = _item.Texture;
		_label.Text = "Lvl " + _item.Level;

		_item.ItemLeveledUpEvent += UpdateFrame;
	}

	/// <summary>
	/// Update the frame with the current item data. Called by the ItemLeveledUp event.
	/// </summary>
	public void UpdateFrame()
	{
		GD.PrintErr("Update");
		_tex.Texture = _item.Texture;
		_label.Text = "Lvl " + _item.Level;
	}

	#endregion

}
