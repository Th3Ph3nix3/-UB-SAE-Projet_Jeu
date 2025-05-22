using Godot;
using System;

public partial class Options : VBoxContainer
{
	#region Attributes
	// Variable to store weapon container
	[Export]
	private HBoxContainer weapons;

	// Preload the option slot 
	private PackedScene OptionSlot = GD.Load<PackedScene>("res://scenes/option_slot.tscn");
	#endregion

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

	public void show_options()
	{
		OptionSlot optionSlot = (OptionSlot)OptionSlot.Instantiate();
		GD.Print(optionSlot.GetType());
		optionSlot.options = this; // téo noobs
		AddChild(optionSlot);

		// Show the option slot and pause the game
		Show();
		GetTree().Paused = true;
	}
}
