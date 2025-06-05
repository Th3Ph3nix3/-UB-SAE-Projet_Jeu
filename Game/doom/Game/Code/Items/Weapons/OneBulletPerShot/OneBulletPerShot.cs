using System;
using Godot;

/// <summary>
/// A pistol weapon that shoots one projectile at a time towards the nearest enemy.
/// </summary>
[GlobalClass]
public partial class OneBulletPerShot : Weapons_Data
{
	/// <summary>
	/// Shoots a projectile towards the nearest enemy of the owner.
	/// </summary>
	public override void EffectUpdate(int level)
	{
		if (holder.nearest_enemy == null)
		{
			GD.PrintErr("target is null");
			return;
		}

		Projectile NewProjectile = projectile.Instantiate<Projectile>();

		NewProjectile.Position = holder.Position;
		NewProjectile.damage = upgrades[level].damage;
		NewProjectile.speed = upgrades[level].speed;
		NewProjectile.source = holder;
		NewProjectile.direction = (holder.nearest_enemy.Position - holder.Position).Normalized();

		holder.GetTree().CurrentScene.AddChild(NewProjectile); // add the projectile to the scene /!\ maybe the enemy targeted is already dead, so godot will display an error, it's normal.
	}
}
