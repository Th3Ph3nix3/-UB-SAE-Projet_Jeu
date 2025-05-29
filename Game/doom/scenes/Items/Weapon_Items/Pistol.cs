using System;
using Godot;

/// <summary>
/// A pistol weapon that shoots one projectile at a time towards the nearest enemy.
/// </summary>
[GlobalClass]
public partial class Pistol : Weapons
{
	/// <summary>
	/// Shoots a projectile towards the nearest enemy of the owner.
	/// </summary>
	protected override void Shoot()
	{
		if (_owner.nearest_enemy == null)
		{
			GD.PrintErr("target is null");
			return;
		}

		Projectile projectile = _projectile_node.Instantiate<Projectile>();

		projectile.Position = _owner.Position;
		projectile.damage = _upgrades[Level].damage;
		projectile.speed = _upgrades[Level].speed;
		projectile.source = _owner;
		projectile.direction = (_owner.nearest_enemy.Position - _owner.Position).Normalized();

		_owner.GetTree().CurrentScene.AddChild(projectile); // add the projectile to the scene /!\ maybe the enemy targeted is already dead, so godot will display an error.
	}
}
