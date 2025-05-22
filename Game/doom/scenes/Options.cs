using Godot;
using System;
using System.Collections.Generic;

public partial class Options : VBoxContainer
{
	#region Attributes

	

	// Preload the option slot 
	private PackedScene OptionSlot = GD.Load<PackedScene>("res://scenes/option_slot.tscn");
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
	#region Methods

	// When the game starts, hide the option slot
	public override void _Ready()
	{
		Hide();
	}

	// Hide the option slot and resume to the scene tree (called by OptionSlot)
	public void close_options()
	{
		Hide();
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
		return weapon_resource;
	}

	public void show_options()
	{
		// OptionSlot optionSlot = (OptionSlot)OptionSlot.Instantiate();
		// GD.Print(optionSlot.GetType());
		// optionSlot.options = this; 
		// AddChild(optionSlot);

		// // Show the option slot and pause the game
		// Show();
		// GetTree().Paused = true;

		var available_weapons = get_available_weapons(); // get the available weapons
		if (available_weapons.Count == 0)
		{
			return; // if there is no weapon to upgrade, return
		}

		foreach (Slot slot in GetChildren())
		{
			slot.QueueFree(); // if there is any weapons, then remove previous ones
		}


		var option_size = 0; // to store how many options are getting added
		foreach (Weapon weapon in available_weapons)
		{
			if (weapon.is_upgradable())
			{
				var optionSlot = (OptionSlot)OptionSlot.Instantiate(); // instantiate the option slot
				optionSlot.weapon = weapon; // set the weapon to the option slot
				AddChild(optionSlot); // add the option slot to the scene
				option_size++;
			}
		}
		if (option_size == 0)
		{
			return; // if none of the weapons can be upgraded, return
		}

		Show();
		GetTree().Paused = true; // pause the game
	}
	#endregion
}
