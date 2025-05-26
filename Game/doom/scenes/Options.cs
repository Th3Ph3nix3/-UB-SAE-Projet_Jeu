using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Options : VBoxContainer
{
	#region Attributes

	/// <summary>
	/// Reference to the player that will be upgraded by the options.
	/// </summary>
	[Export]
	private PlayerControl _player;

	/// <summary>
	/// Preload the option slot.
	/// </summary>
	private PackedScene _optionSlot = GD.Load<PackedScene>("res://scenes/option_slot.tscn");

	/// <summary>
	/// Preload the passive slot.
	/// </summary>
	private PackedScene _passiveSlot = GD.Load<PackedScene>("res://scenes/PassiveSlot.tscn");

	/// <summary>
	/// To add particles in the background when a level up happen.
	/// </summary>
	[Export]
	private GpuParticles2D _particles;

	/// <summary>
	/// To add a panel in the background when a level up happen.
	/// </summary>
	[Export]
	private NinePatchRect _panel;

	/// <summary>
	/// Variable to store weapon container.
	/// </summary>
	[Export]
	private HBoxContainer _weapons;

	/// <summary>
	/// Variable to store passive items container. Which contains the passive items that can be upgraded.
	/// </summary>
	[Export]
	private HBoxContainer passive_items;

	#endregion

	#region _Ready()

	/// <summary>
	/// Called when the node enters the scene tree for the first time.
	/// Initialize its variable and hide the particules and panel.
	/// </summary>
	public override void _Ready()
	{
		// Hide things when enter the game
		Hide();
		_particles.Hide();
		_panel.Hide();
	}

	#endregion

	#region Getter / Setter

	public PlayerControl Player
	{
		get => _player;
	}

	#endregion

	#region Methods

	#region Private Methods
	/// <summary>
	/// Get the weapon and passive currently equipped on the player.
	/// </summary>
	/// <returns></returns>
	private List<PanelContainer> get_available_resource()
	{
		List<PanelContainer> resources = new List<PanelContainer>();

		foreach (Slot child in _weapons.GetChildren())
		{
			if (child is Slot slot && slot.Weapon != null)
			{
				resources.Add(slot);
			}
		}

		foreach (PassiveSlot child in passive_items.GetChildren())
		{
			if (child is PassiveSlot passiveSlot && passiveSlot.Item != null)
			{
				resources.Add(passiveSlot);
			}
		}

		return resources;
	}

	/// <summary>
	/// Add all necessary options slots for the given item and weapon.
	/// </summary>
	/// <param name="item">Item to add in option</param>
	/// <returns>Returns 1 if the option was added, 0 otherwise.</returns>
	private int add_options(Item item)
	{
		if (item is Weapon weapon)
		{
			if (weapon.is_upgradable())
			{
				OptionSlot option_slot = (OptionSlot)_optionSlot.Instantiate();
				AddChild(option_slot);

				option_slot.weapon = weapon;
				option_slot.options = this;

				return 1;
			}
		}
		else if (item is PassiveItem passiveItem)
		{
			if (passiveItem.is_upgradable())
			{
				OptionSlot option_slot = (OptionSlot)_optionSlot.Instantiate();
				AddChild(option_slot);

				option_slot.passive_item = passiveItem;
				option_slot.options = this;

				return 1;
			}
		}

		return 0;
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Show the options for the player to upgrade its weapons
	/// </summary>
	public void show_options()
	{
		// Get the available weapons and passive items
		List<PanelContainer> available_items = get_available_resource();
		
		if (available_items.Count == 0)
		{
			// If there is no weapon or passive item to upgrade, return
			return;
		}

		// Clean ALL previous children (upgrade options)
		foreach (Node child in GetChildren())
		{
			if (child is OptionSlot)
			{
				RemoveChild(child);
				child.QueueFree();
			}
		}

		// To count how many options are getting added
		int option_size = 0;

		// Add Passive items and weapons options
		foreach (PanelContainer slot in available_items)
		{
			if (slot is Slot slotWeapon && slotWeapon.Weapon != null)
			{
				option_size += add_options(slotWeapon.Weapon);
			}
			else if (slot is PassiveSlot passiveSlot && passiveSlot.Item != null)
			{
				option_size += add_options(passiveSlot.Item);
			}
		}
		
		if (option_size == 0)
		{
			// If none of the weapons/items can be upgraded, return
			return;
		}

		// Show the upgrade panel
		_particles.Show();
		_panel.Show();
		Show();
		GetTree().Paused = true;
	}

	/// <summary>
	/// Hide the panel and particles, and resume to the scene tree (called by OptionSlot)
	/// </summary>
	public void close_options()
	{
		Hide();
		_particles.Hide();
		_panel.Hide();
		GetTree().Paused = false;
	}

	#endregion

	#endregion
}
