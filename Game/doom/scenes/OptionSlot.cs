using Godot;
using System;
using System.Reflection.Emit;

public partial class OptionSlot : TextureButton
{
	#region Attributes
	private Options _options;
	#endregion

	#region Setters / Getters

	public Options options
	{
		get => _options;
		set
		{
			_options = value;
		}
	}
	
	[Export]
	private Weapon _weapon;
	public Weapon weapon
	{
		set
		{
			_weapon = value;
			if (value != null)
			{
				Godot.Label label = GetNode<Godot.Label>("Label");
				Godot.Label description = GetNode<Godot.Label>("Description");
				TextureNormal = value.Texture;
				label.Text = "Lvl " + (_weapon.Level).ToString();
				description.Text = value.Upgrades[value.Level - 1].description;
			}
		}
	}

	[Export]
	private PassiveItem _passive_item;
	public PassiveItem passive_item
	{
		set
		{
			_passive_item = value;
			if (value != null)
			{
				Godot.Label label = GetNode<Godot.Label>("Label");
				Godot.Label description = GetNode<Godot.Label>("Description");
				TextureNormal = value.Texture;
				label.Text = "Lvl " + (_passive_item.Level).ToString();
				description.Text = value.Upgrades[value.Level - 1].description;
			}
		}
	}

	#endregion

	#region Methods

	public void _on_gui_input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("click"))
		{
			if (_weapon != null)
			{
				_weapon.UpgradeItem(); // upgrade the weapon
			}
			
			if (_passive_item != null)
			{
				_passive_item.upgrade_item(_options.Player); // upgrade the passive item
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
