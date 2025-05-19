using Godot;
using System;
using System.Diagnostics;
using System.Runtime;

[GlobalClass]
public partial class SingleShot : Weapon
{
    public void shoot(PlayerControl source, Enemy target, SceneTree scene_tree)
    {
        if (target == null)
        {
            return; // if there is no target to shot at, do nothing
        }

        // else, do that : 
        var projectile = projectile_node.Instantiate<Projectile>(); // instantiate a projectile

        projectile.Position = source.Position; // position of the player
        projectile.damage = damage;
        projectile.speed = speed;
        projectile.direction = (target.Position - source.Position).Normalized(); // go to nearest enemy (target) at a certain speed

        source.GetTree().CurrentScene.AddChild(projectile);
    }

    public override void Activate(PlayerControl source, Enemy target, SceneTree scene_tree)
    {
        shoot(source, target, scene_tree); // call the shoot method
    }
}
