using Godot;
using System;
using System.Xml.Resolvers;

[GlobalClass]
public abstract partial class Weapon : Resource
{
	#region attributes
	[Export]
	public string title; // name of the weapon

	[Export]
	public Texture2D texture; // icon of the weapon

	// properties of the projectile / ! \ to change those values, double click on .tres file on the inspector and directly change in Godot
	[Export]
	public float damage; // damage of the projectile
	[Export]
	public float cooldown; // cooldown until next projectile
	[Export]
	public float speed; // speed of the projectile

	[Export]
	public PackedScene projectile_node = GD.Load<PackedScene>("res://scenes/projectile.tscn"); // scene of the projectile

	[Export]
	public ProjectileUpgrade[] upgrades = Array.Empty<ProjectileUpgrade>(); // Array of upgrades for the weapon

	public int level = 1; // level of the weapon

	#endregion
	#region methods
	public abstract void Activate(PlayerControl _source, Enemy _target, SceneTree _scene_tree); // abstract method, overriden in SingleShot.cs
	public bool is_upgradable()
	{
		return level <= upgrades.Length; // check if the weapon can be upgraded
	}

	public abstract void UpgradeItem(); // abstract method, overriden in SingleShot.cs

	#endregion
}
