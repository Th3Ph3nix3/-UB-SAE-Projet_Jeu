using Godot;
using System;
using System.Collections.Generic;

public partial class Options : VBoxContainer
{
	#region Attributes
	// Preload the option slot 
	private PackedScene OptionSlot = GD.Load<PackedScene>("res://scenes/option_slot.tscn");

	[Export]
	private GpuParticles2D particles;

	[Export]
	private NinePatchRect panel;
	#endregion

	#region Setters / Getters
	// Variable to store weapon container
	[Export]
	private HBoxContainer _weapons;

	public HBoxContainer weapons
	{
		get => _weapons;
		set
		{
			_weapons = value;
		}
	}
	#endregion
	#region Ready

	// When the game starts, hide the option slot
	public override void _Ready()
	{
		// Hide things when enter the game
		Hide();
		particles.Hide();
		panel.Hide();
	}
	#endregion
	#region Methods

	// Hide the option slot and resume to the scene tree (called by OptionSlot)
	public void close_options()
	{
		Hide();
		particles.Hide();
		panel.Hide();
		GetTree().Paused = false;
	}

	public List<Node> get_available_weapons()
	{
		List<Node> weapon_resource = new List<Node>();
		foreach (Node w in weapons.GetChildren())
		{
			if (w != null)
			{
				weapon_resource.Add(w); // /!\ maybe a problem here
			}
		}
		// GD.PrintErr("weapons are " + weapon_resource.Count);
		return weapon_resource;
	}

	public void show_options()
	{
		var available_weapons = get_available_weapons(); // get the available weapons
		if (available_weapons.Count == 0)
		{
			return; // if there is no weapon to upgrade, return
		}

		foreach (OptionSlot slot in GetChildren())
		{
			slot.QueueFree(); // if there is any weapons, then remove previous ones
		}

		var option_size = 0; // to store how many options are getting added
		foreach (Slot Slots in available_weapons)
		{
			if (Slots.weapon.is_upgradable())
			{
				OptionSlot optionSlot = (OptionSlot)OptionSlot.Instantiate(); // instantiate the option slot
				optionSlot.weapon = Slots.weapon; // set the weapon to the option slot
				optionSlot.options = this; // set the options to the option slot so it can call the close_options function
				AddChild(optionSlot); // add the option slot to the scene
				option_size++;
			}
		}

		if (option_size == 0)
		{
			return; // if none of the weapons can be upgraded, return
		}

		Show();

		// show that while showing options
		particles.Show();
		panel.Show();
		GetTree().Paused = true; // pause the game
	}
	#endregion
}
