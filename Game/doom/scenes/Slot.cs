using Godot;
using System;
using System.Net.WebSockets;

public partial class Slot : PanelContainer
{
	#region attributes

	public TextureRect tex;
	public Timer Cooldown;

	#endregion

	#region setters / getters
		
	[Export]
	public Weapon _weapon;
	public Weapon weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			tex.Texture = value.Texture; // updating texturect and wait time for the timer
			this.Cooldown.WaitTime = value.Cooldown; // cooldown until weapon can be used
		}
	}

	#endregion

	#region methods

	public override void _Ready()
	{
		tex = GetNode<TextureRect>("TextureRect");
		this.Cooldown = GetNode<Timer>("Cooldown");

		if (weapon != null)
		{
			weapon = _weapon; // activate setter
		}
	}


	public void _on_cooldown_timeout() {
		if (weapon != null) {
			this.Cooldown.WaitTime = weapon.Cooldown;

			PlayerControl ownerPlayer = GetParent().GetParent().GetParent() as PlayerControl; // cast Owner as PlayerControl (usualy type Node)
			if (ownerPlayer == null) {
				GD.Print("ownerplayer is null or have not been casted in a good way"); // print error message
			}
			
			weapon.Activate(ownerPlayer, ownerPlayer.nearest_enemy, GetTree()); // func is defined like that : public void activate(PlayerControl _source, Enemy _target, SceneTree _scene_tree)
		}
	}
	#endregion
}
