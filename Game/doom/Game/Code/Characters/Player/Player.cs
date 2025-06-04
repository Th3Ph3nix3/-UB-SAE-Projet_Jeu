using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public partial class Player : CharacterBody2D
{

	#region attributes
	[Export]
	public float movement_speed { get; set; } = 400;
	private ProgressBar healthBar;
	public int total_XP = 0;
	public float recovery = 0;
	public float armor = 0;
	public float might = 1f; // public so that Projectile.cs can access to it
	public float area = 500; // area from where the player start shooting at enemies ( = range)
	public int growth = 1;
	public Enemy nearest_enemy;
	public CollisionShape2D magnetArea;
	public float nearest_enemy_distance; // float.PositiveInfinity est la représentation de l'infini

	/// <summary>
	/// Current health of the player.
	/// </summary>
	private int _health = Global.BASE_MAX_PLAYER_HEALTH;

	/// <summary>
	/// Max health the player can have at any given point.
	/// </summary>
	private int _maxHealth = Global.BASE_MAX_PLAYER_HEALTH;

	/// <summary>
	/// Current level of the player.
	/// </summary>
	private int _level = 0;

	/// <summary>
	/// Current Xp of the player.
	/// </summary>
	private int _xp = 0;

	/// <summary>
	/// Xp needed to level up to the next level.
	/// </summary>
	private int _nextLevelXpNeeded = Global.BASE_XP_TO_GET;

	/// <summary>
	/// Weapons that the player currently have.
	/// </summary>
	private Items _weapon = new Items();

	/// <summary>
	/// Passives that the player currently have.
	/// </summary>
	private List<Items> _passives = new();

	#endregion

	#region Events

	/// <summary>
	/// Event raised when the player level up.
	/// </summary>
	public event EventHandler<LevelUpEventArgs> LevelUpEvent;

	/// <summary>
	/// Class used to pass the LevelUp event data.
	/// Contains the current level of the player.
	/// </summary>
	public class LevelUpEventArgs : EventArgs
	{
		public int CurrentLvl { get; }
		public int CurrentNextLevelXpNeeded { get; }
		public LevelUpEventArgs(int currentlvl, int currentNextLevelXpNeeded)
		{ CurrentLvl = currentlvl; CurrentNextLevelXpNeeded = currentNextLevelXpNeeded; }
	}

	/// <summary>
	/// Event raised when the player gaines Xp.
	/// </summary>
	public event EventHandler<XpGainedEventArgs> XpGainedEvent;

	/// <summary>
	/// Class used to pass the XpGained event data.
	/// Contains the current xp of the player and how much xp he will need for this level.
	/// </summary>
	public class XpGainedEventArgs : EventArgs
	{
		public int CurrentXp { get; }
		public XpGainedEventArgs(int currentXp)
		{ CurrentXp = currentXp; }
	}

	/// <summary>
	/// Event raised when an item is added to the player.
	/// </summary>
	public event EventHandler<ItemAddedEventArgs> ItemAddedEvent;

	/// <summary>
	/// Class used to pass the ItemAdded event data.
	/// Contains the item added to the player.
	/// </summary>
	public class ItemAddedEventArgs : EventArgs
	{
		public Items AddedItem { get; }
		public ItemAddedEventArgs(Items addedItem)
		{ AddedItem = addedItem; }
	}

	/// <summary>
	/// Event raised when the player lose or gain health.
	/// </summary>
	public event EventHandler<HealthChangedEventArgs> HealthChangedEvent;

	/// <summary>
	/// Class used to pass the HealthChanged event data.
	/// Contain the current health of the player.
	/// </summary>
	public class HealthChangedEventArgs : EventArgs
	{
		public int CurrentHealth { get; }
		public int CurrentMaxHealth { get; }
		public HealthChangedEventArgs(int currentHealth, int currentMaxHealth)
		{ CurrentHealth = currentHealth; CurrentMaxHealth = currentMaxHealth; }
	}

	#endregion

	#region Setters / Getters

	private float _magnet = 0;
	public float magnet
	{
		get => _magnet;
		set
		{
			_magnet = value;

			if (magnetArea.Shape is CircleShape2D circleShape)
			{
				circleShape.Radius = 50 + value; // change the radius of the object detecion
			}
		}
	}

	public int Health
	{
		get => _health;
		set
		{
			if (value < 0) _health = 0;
			else if (value > MaxHealth) _health = MaxHealth;
			else _health = value;

			HealthChangedEvent?.Invoke(this, new HealthChangedEventArgs(_health, _maxHealth));
		}
	}
	public int MaxHealth
	{
		get => _maxHealth;
		set
		{
			if (value < 1) _maxHealth = 1;
			else _maxHealth = value;

			HealthChangedEvent?.Invoke(this, new HealthChangedEventArgs(_health, _maxHealth));
		}
	}
	public int Level { get => _level; }
	public Items Weapon { get => _weapon; }
	public List<Items> Passives { get => _passives; }

	#endregion

	#region methods

	public void _on_magnet_area_entered(Area2D area) // to attract bonus and xp rewards.
	{
		if (area.HasMethod("follow"))
		{
			var Area = area as Pickups;
			Area.follow(this);
		}
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down"); // moves of the player
		Velocity = inputDirection * movement_speed;

		if (Input.IsActionPressed("esc"))
		{
			GetTree().Quit();
		}
	}

	/// <summary>
	/// Function called when the body of an enemy touch the player.
	/// The player take damage accordingly.
	/// </summary>
	/// <param name="body">Enemy which touched the player.</param>
	public void _on_self_damage_body_entered(Enemy body)
	{
		int Damage = (int)(body.Damage - armor);
		Health = _health - Damage;
	}

	/// <summary>
	/// Give a certain amount of xp to the player, raise the XpGained event and LevelUp event if the player level up.
	/// </summary>
	/// <param name="amount">Amount of xp to give to the player.</param>
	public void GainXp(int amount)
	{
		// /!\ To change later, so the player can gain multiple level at once.
		if (_xp + amount > _nextLevelXpNeeded)
		{
			_level += 1;
			_xp = 0;
			_nextLevelXpNeeded = Global.BASE_XP_TO_GET + Level * Global.XP_SCALE;

			LevelUpEvent?.Invoke(this, new LevelUpEventArgs(_level, _nextLevelXpNeeded));
		}
		else _xp += amount;

		XpGainedEvent?.Invoke(this, new XpGainedEventArgs(_xp));
	}

	/// <summary>
	/// Add a new passive to the player.
	/// </summary>
	/// <param name="item">Item to add.</param>
	public void AddItem(Items item)
	{
		if (item.Type == Items_Type.Passive)
		{
			item.Holder = this;
			_passives.Add(item);
		}
		else if (item.Type == Items_Type.Weapon)
		{
			_weapon = item;
		}

		ItemAddedEvent?.Invoke(this, new ItemAddedEventArgs(item));
	}

	/// <summary>
	/// Handles the player animation based on movement and direction.
	/// </summary>
	private void Animation()
	{
		if (Velocity != Godot.Vector2.Zero)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Walk");
		}
		else
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("Idle");
		}
		if (Velocity.X > 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
		}
		else if (Velocity.X < 0)
		{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
		}
	}

	#endregion

	#region Physics Process() and Ready()
	public override void _PhysicsProcess(double delta)
	{
		if (IsInstanceValid(nearest_enemy))
		{ // if the player is near an enemy
			nearest_enemy_distance = nearest_enemy.Separation;
		}
		else
		{
			nearest_enemy_distance = 150 + area; ; // else (there is no enemy near the player), the distance 150 (can be changed if not appropriate) + area radius
			nearest_enemy = null; // reset reference because no near enemy has been found 
		}

		GetInput();
		MoveAndSlide();
		Animation();

		foreach (Items passive in _passives) { passive.UpdateEffectTimer(delta); }
		_weapon.UpdateEffectTimer(delta);
	}

	// used to instantiate players attributes on the game scene 
	public override void _Ready()
	{
		nearest_enemy_distance = 150 + area; // set nearest enemy distance to 150 + area (go in attributes to learn more)
		magnetArea = GetNode<CollisionShape2D>("Magnet/MagnetZone"); // magnet

		AddItem(new Items(GD.Load<Weapons_Data>("res://Game/Resource/Weapons/Pistol.tres")));
		_weapon.Holder = this; // set the owner of the weapons container to this player
	}

	#endregion

}
