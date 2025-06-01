using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Options : VBoxContainer
{
	#region Attributes

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

	#region Methods

	#region Public Methods

	/// <summary>
	/// Show the options for the player to upgrade its weapons
	/// </summary>
	public void show_options()
	{
		List<Items> ItemsOfPlayer = [.. Global.PlayerManager.Player.Passives];
		ItemsOfPlayer.Add(Global.PlayerManager.Player.Weapon);

		// Clean ALL previous children (upgrade options)
		foreach (Node child in GetChildren())
		{
			if (child is Option_Frame)
			{
				RemoveChild(child);
				child.QueueFree();
			}
		}

		bool flag = false;

		foreach (Items item in ItemsOfPlayer)   
		{
			if (item.IsUpgradable) { AddChild(Option_Frame.new_OptionSlot(item, this)); flag = true; }
		}

		if (flag)
		{
			// Show the upgrade panel
			_particles.Show();
			_panel.Show();
			Show();
			GetTree().Paused = true;
		}
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
