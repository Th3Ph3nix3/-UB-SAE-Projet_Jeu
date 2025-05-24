using Godot;
using System;
using System.Net.WebSockets;

public partial class PassiveSlot : PanelContainer
{
	#region attributes

	public TextureRect tex;

	#endregion

	#region setters / getters
		
	[Export]
	public Item _item;
	public Item item
	{
		get => _item;
		set
		{
			_item = value;
			if (tex != null && value != null)
				tex.Texture = value.texture;
		}
	}


	#endregion

	#region methods

	public override void _Ready()
	{
		tex = GetNode<TextureRect>("TextureRect");

		if (item != null)
		{
			tex.Texture = _item.texture; // activate setter
		}
	}
	#endregion
}
