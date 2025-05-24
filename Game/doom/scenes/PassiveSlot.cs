using Godot;
using System;

[GlobalClass]
public partial class PassiveSlot : PanelContainer
{
	[Export]
	private PassiveItem _item; // store the passive item
	private TextureRect texture;
	public PassiveItem item
	{
		get => _item;
		set
		{
			item = value;
			texture.Texture = value.texture;
		}
	}

	public override void _Ready()
	{
		texture = GetNode<TextureRect>("TextureRect");

		if (item != null)
		{
			item.player_reference = (PlayerControl)Owner;
		}
	}

}
