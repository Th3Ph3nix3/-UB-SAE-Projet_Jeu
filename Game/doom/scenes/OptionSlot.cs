using Godot;
using System;
using System.Reflection.Emit;

public partial class OptionSlot : TextureButton
{
	#region Attributes

	/// <summary>
	/// Reference to the options class to call _close_options() method when the player click on the slot.
	/// </summary>
	private Options _options;

	/// <summary>
	/// Weapon holded in this slot. Can't be a weapon and a passive item at the same time.
	/// </summary>
	[Export]
	private Weapon _weapon;

	/// <summary>
	/// Passive item holded in this slot. Can't be a weapon and a passive item at the same time.
	/// </summary>
	[Export]
	private PassiveItem _passive_item;

	#endregion

	#region Setters / Getters

	public Options Options
	{
		get => _options;
		set
		{
			if (value is Options && value != null)
			{
				_options = value;
			}
		}
	}
	public Weapon weapon
	{
		set
		{
			if (value != null)
			{
				_weapon = value;

				Godot.Label label = GetNode<Godot.Label>("Label");
				Godot.Label description = GetNode<Godot.Label>("Description");

				TextureNormal = value.Texture;
				label.Text = "Lvl " + (_weapon.Level).ToString();
				description.Text = value.Upgrades[value.Level - 1].description;

				_passive_item = null; // reset the passive item if a weapon is set
			}
		}
	}

	public PassiveItem passive_item
	{
		set
		{

			if (value != null)
			{
				_passive_item = value;

				Godot.Label label = GetNode<Godot.Label>("Label");
				Godot.Label description = GetNode<Godot.Label>("Description");

				TextureNormal = value.Texture;
				label.Text = "Lvl " + (_passive_item.Level).ToString();
				description.Text = value.Upgrades[value.Level - 1].description;

				_weapon = null; // reset the weapon if a passive item is set
			}
		}
	}

	#endregion

	#region Methods

	/// <summary>
	/// Called when the player clicks on the slot to upgrade the weapon or passive item.
	/// </summary>
	/// <param name="inputEvent">Input catched</param>
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
