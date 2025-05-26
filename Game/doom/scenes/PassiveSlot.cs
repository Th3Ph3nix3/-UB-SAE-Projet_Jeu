using Godot;
using System;
using System.Net.WebSockets;

public partial class PassiveSlot : PanelContainer
{
	#region attributes

	/// <summary>
	/// Texture of the passive item to be displayed in this slot.
	/// </summary>
	private TextureRect _tex;

	/// <summary>
	/// Passive item to be displayed in this slot.
	/// </summary>
	[Export]
	private PassiveItem _item;

	#endregion

	#region setters / getters

	public PassiveItem Item
	{
		get => _item;
	}

	#endregion

	#region methods

	/// <summary>
	/// Called when the node is added to the scene. Load the texture of the item on the scene.
	/// </summary>
	public override void _Ready()
	{
		_tex = GetNode<TextureRect>("TextureRect");

		if (_tex != null && _item != null)
		{
			_tex.Texture = _item.Texture; // update texture
		}
	}

	#endregion
}
