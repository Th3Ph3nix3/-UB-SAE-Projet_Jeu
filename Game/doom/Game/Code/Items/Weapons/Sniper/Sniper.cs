using Godot;
using System;

[GlobalClass]
public partial class Sniper : Weapons_Data
{
	public override void EffectUpdate()
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
