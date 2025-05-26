using Godot;
using System;
using System.Net.WebSockets;

public partial class Slot : PanelContainer
{
	#region attributes

	/// <summary>
	/// Texture of the weapon to be displayed in this slot.
	/// </summary>
	private TextureRect _tex;

	/// <summary>
	/// Weapon to be displayed in this slot.
	/// </summary>
	[Export]
	public Weapon _weapon;

	/// <summary>
	/// Cooldown timer for the weapon activation.
	/// </summary>
	private Timer _cooldown;

	[Export]
	private PlayerControl _owner;

	#endregion

	#region setters / getters

	public Weapon Weapon
	{
		get => _weapon;
	}

	#endregion

	#region methods

	/// <summary>
	/// Called when the node is added to the scene. Load the texture of the item on the scene.
	/// Also set the cooldown timer to the weapon's cooldown time.
	/// </summary>
	public override void _Ready()
	{
		_tex = GetNode<TextureRect>("TextureRect");
		this._cooldown = GetNode<Timer>("Cooldown");

		if (_tex != null && _weapon != null)
		{
			_tex.Texture = _weapon.Texture; // update texture
			this._cooldown.WaitTime = _weapon.Cooldown; // update cooldown timer
		}
	}

	/// <summary>
	/// Called when the cooldown timer times out. This method activates the weapon.
	/// </summary>
	public void _on_cooldown_timeout() {
		if (Weapon != null) 
		{
			this._cooldown.WaitTime = Weapon.Cooldown;
			Weapon.Activate(_owner, _owner.nearest_enemy, GetTree()); // func is defined like that : public void activate(PlayerControl _source, Enemy _target, SceneTree _scene_tree)
		}
	}
	#endregion
}
