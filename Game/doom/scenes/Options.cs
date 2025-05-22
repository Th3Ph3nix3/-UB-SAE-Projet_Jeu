using Godot;
using System;
using System.Collections.Generic;

public partial class Options : VBoxContainer
{
	#region Attributes

	/// <summary>
	/// Preload the option slot.
	/// </summary>
	private PackedScene _optionSlot = GD.Load<PackedScene>("res://scenes/option_slot.tscn");

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

	#endregion

	#region Ready

	/// <summary>
	/// Called when the node enters the scene tree for the first time.
	/// Initialize its variable and hide the particules and panel.
	/// </summary>
	public override void _Ready()
	{
		_weapons = GetNode<HBoxContainer>("../Weapons");
		_particles = GetNode<GpuParticles2D>("../Particles");
		_panel = GetNode<NinePatchRect>("../Panel");

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

	/// <summary>
	/// Get the available weapons for upgrade
	/// </summary>
	/// <returns>List of the available weapons</returns>
	public List<Slot> get_available_weapons()
	{
		List<Slot> weapon_resource = new List<Slot>();
		foreach (Slot w in _weapons.GetChildren())
		{
			if (w != null)
			{
				weapon_resource.Add(w);
			}
		}

		return weapon_resource;
	}

	/// <summary>
	/// Show the options for the player to upgrade its weapons
	/// </summary>
	public void show_options()
	{
		// Get the available weapons
		List<Slot> available_weapons = get_available_weapons();
		if (available_weapons.Count == 0)
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
			if (Slots.weapon.is_upgradable())
			{
				OptionSlot optionSlot = (OptionSlot)_optionSlot.Instantiate();
				optionSlot.weapon = Slots.weapon;
				// Set the options to the option slot so it can call the close_options function
				optionSlot.options = this;
				AddChild(optionSlot); 
				option_size++;
			}
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
