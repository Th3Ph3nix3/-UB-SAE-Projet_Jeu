using Godot;
using System;
using System.Net.WebSockets;

public partial class Slot : PanelContainer
{
	
	[Export]
	public Weapon _weapon;

	public TextureRect tex;
	public Timer Cooldown;

	public override void _Ready()
	{
		tex = GetNode<TextureRect>("TextureRect");
		Cooldown = GetNode<Timer>("Cooldown");

		if(weapon != null){
			weapon = _weapon; // activate setter
		}
	}

	public Weapon weapon
    {
        get => _weapon;
        set
        {
            _weapon = value;
			tex.Texture = value.texture; // updating texturect and wait time for the timer
			Cooldown.WaitTime = value.cooldown;
        }
    }

	public void _on_cooldown_timeout(){
		if(weapon != null){
			Cooldown.WaitTime = weapon.cooldown;

			var ownerPlayer = GetParent().GetParent().GetParent() as PlayerControl; // cast Owner as PlayerControl (usualy type Node)
			if(ownerPlayer == null){
				GD.Print("ownerplayer is null or have not been casted in a good way");
			}
			weapon.Activate(ownerPlayer,ownerPlayer.nearest_enemy,GetTree()); // func is defined like that : public void activate(PlayerControl _source, Enemy _target, SceneTree _scene_tree)
		}
	}
}
