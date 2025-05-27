using Godot;
using System;
using System.Diagnostics;
using System.Runtime;

[GlobalClass]
public partial class SingleShot : Weapon
{
	#region method
	public void shoot(PlayerControl source, Enemy target, SceneTree scene_tree)
	{
		if (target == null)
		{
			GD.PrintErr("target is null"); // print error message
			return; // if there is no target to shot at, do nothing
		}

		// else, do that : 
		Projectile projectile = Projectile_node.Instantiate<Projectile>(); // instantiate a projectile

		projectile.Position = source.Position; // position of the player
		projectile.damage = Damage;
		projectile.speed = Speed;
		projectile.source = source; // set a source for the projectile
		projectile.direction = (target.Position - source.Position).Normalized(); // go to nearest enemy (target) at a certain speed

		source.GetTree().CurrentScene.AddChild(projectile); // add the projectile to the scene /!\ maybe the enemy targeted is already dead, so godot will display an error.
	}

	public override void Activate(PlayerControl source, Enemy target, SceneTree scene_tree)
	{
		shoot(source, target, scene_tree); // call the shoot method
	}

	public override void UpgradeItem()
	{
		if (!is_upgradable())
		{
			return; // if the weapon can't be upgraded, return
		}

		ProjectileUpgrade CurrentUpgrades = Upgrades[Level - 1];

		// Upgrading the weapon stats
		Damage += CurrentUpgrades.damage;
		Cooldown += CurrentUpgrades.cooldown;
		Speed += CurrentUpgrades.speed;

		Level++; // increase the level of the weapon
	}
	#endregion
}
