using Godot;
using System;
using System.Xml.Resolvers;

[GlobalClass]
public abstract partial class Weapon : Item
{
	#region attributes

	/// <summary>
	/// Title of the weapon.
	/// </summary>
	[Export]
	private string _title;

	// Properties of the projectile / ! \
	// To change those values, double click on .tres file on the inspector and directly change in Godot
	#region projectile properties

	/// <summary>
	/// Damage of the projectile.
	/// </summary>
	[Export]
	private float _damage;

	/// <summary>
	/// Cooldown of the weapon before it shoots again.
	/// </summary>
	[Export]
	private float _cooldown;

	/// <summary>
	/// Speed of the projectile.
	/// </summary>
	[Export]
	private float _speed;

	/// <summary>
	/// Level of the weapon.
	/// </summary>
	private int _level = 1;

	#endregion

	/// <summary>
	/// Load the packedScene of the projectile to display it.
	/// </summary>
	[Export]
	private PackedScene _projectile_node = GD.Load<PackedScene>("res://scenes/projectile.tscn");

	/// <summary>
	/// Array containing the upgrade path, each index per level, for the weapon.
	/// </summary>
	[Export]
	private ProjectileUpgrade[] _upgrades = Array.Empty<ProjectileUpgrade>();

	#endregion

	#region Getter / Setter

	public string Title
	{
		get => _title;
	}
	public float Damage
	{
		get => _damage;
		set => _damage = value;
	}
	public float Cooldown
	{
		get => _cooldown;
		set => _cooldown = value;
	}
	public float Speed
	{
		get => _speed;
		set => _speed = value;
	}
	public int Level
	{
		get => _level;
		set => _level = value;
	}
	public PackedScene Projectile_node
	{
		get => _projectile_node;
	}
	public ProjectileUpgrade[] Upgrades
	{
		get => _upgrades;
	}

	#endregion

	#region methods

	/// <summary>
	/// Overriden in SingleShot.cs
	/// </summary>
	/// <param name="_source">The source of the shot</param>
	/// <param name="_target">The target of the shot</param>
	public abstract void Activate(PlayerControl _source, Enemy _target, SceneTree _scene_tree);

	/// <summary>
	/// To know if the weapon have still an upgrade
	/// </summary>
	/// <returns>True if yes, False if no</returns>
	public bool is_upgradable()
	{
		return Level <= Upgrades.Length; // check if the weapon can be upgraded
	}

	/// <summary>
	/// overriden in SingleShot.cs
	/// </summary>
	public abstract void UpgradeItem();

	#endregion
}
