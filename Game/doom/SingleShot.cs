using Godot;
using System;
using System.Diagnostics;
using System.Runtime;

public partial class SingleShot : Weapon
{
    public void shoot(Enemy source, PlayerControl target, PackedScene scene_tree)
    {
        if (target == null)
        {
            return;
        }

        var projectile = projectile_node.Instantiate();

        projectile.position = source.Position;
        projectile.damage = damage;
        projectile.speed = speed;
        projectile.direction = (target.Position - source.Position).Normalized();

        scene_tree.current_scene.AddChild(projectile);

        // POURQUOI çA MET DES ERREURS JE COMPRENDS PAS >:(
    }
}
