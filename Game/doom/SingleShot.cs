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
            return;
        }


        var projectile = projectile_node.Instantiate<Projectile>();

        projectile.Position = source.Position;
        projectile.damage = damage;
        projectile.speed = speed;
        projectile.direction = (target.Position - source.Position).Normalized();

        source.GetTree().CurrentScene.AddChild(projectile);
    }

    public override void Activate(PlayerControl source, Enemy target, SceneTree scene_tree)
    {
        shoot(source, target, scene_tree);
    }
}
