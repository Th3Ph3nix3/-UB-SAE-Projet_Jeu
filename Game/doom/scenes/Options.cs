using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Options : VBoxContainer
{
	#region Attributes

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
		_weapons = GetNode<HBoxContainer>("../Weapons");
		_particles = GetNode<GpuParticles2D>("../Particles");
		_panel = GetNode<NinePatchRect>("../Panel");
		passive_items = GetNode<HBoxContainer>("../PassiveItems");

		// Hide things when enter the game
		Hide();
		_particles.Hide();
		_panel.Hide();
	}

	#endregion

	#region Methods

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

	public List<Slot> get_available_resource()
	{
		List<Slot> resources = new List<Slot>();

		foreach (Node child in passive_items.GetChildren())
		{
			if (child is Slot slot && slot.weapon != null)
			{
				resources.Add(slot);
			}
		}
		return resources;
	}


	public List<PassiveSlot> get_available_resource_slot()
	{
		List<PassiveSlot> pi = new List<PassiveSlot>();

		foreach (PassiveSlot passiveSlot in passive_items.GetChildren())
		{
			if (passiveSlot.item != null)
			{
				pi.Add(passiveSlot);
			}
		}
		return pi;
	}
	

	public int add_options_weapon(Weapon weapon)
	{
		if (weapon.is_upgradable())
		{
			Slot option_slot = _optionSlot.Instantiate<Slot>();
			option_slot.weapon = weapon;
			AddChild(option_slot);
			return 1;
		}
		return 0;
	}

	public int add_options_item(Item item)
	{
		if (item is PassiveItem passiveItem && passiveItem.is_upgradable())
		{
			PassiveSlot passiveSlot = _passiveSlot.Instantiate<PassiveSlot>();
			passiveSlot.item = item;
			AddChild(passiveSlot);
			return 1;
		}
		return 0;
	}

	/// <summary>
	/// Show the options for the player to upgrade its weapons
	/// </summary>
	public void show_options()
	{
		// Get the available weapons
		List<Slot> available_weapons = get_available_resource();
		List<PassiveSlot> available_passive_item = get_available_resource_slot();
		if (available_weapons.Count == 0 && available_passive_item.Count == 0)
		{
			// If there is no weapon to upgrade, return
			return;
		}

		foreach (OptionSlot slot in GetChildren())
		{
			// Clean the previous upgrade options
			slot.QueueFree();
		}

		// To count how many options are getting added
		int option_size = 0;
		foreach (Slot Slots in available_weapons)
		{
			option_size += add_options_weapon(Slots.weapon);
		}

		foreach (PassiveSlot passiveSlot in available_passive_item)
		{
			option_size += add_options_item(passiveSlot.item);
		}

		if (option_size == 0)
		{
			// If none of the weapons can be upgraded, return
			return;
		}

		// Show that while showing options
		_particles.Show();
		_panel.Show();
		Show();
		GetTree().Paused = true;
	}

	#endregion
}
