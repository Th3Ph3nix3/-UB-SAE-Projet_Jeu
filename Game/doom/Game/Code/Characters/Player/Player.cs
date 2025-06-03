using Godot;
using System;
using System.Collections.Generic;
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
    /// Display the player's current level.
    /// </summary>
    private Label LevelLabel;

    /// <summary>
    /// Displays the player's xp as a progress bar.
    /// </summary>
	private TextureProgressBar xpBar;

	/// <summary>
	/// Weapons that the player currently have.
	/// </summary>
	private Items _weapon;

	/// <summary>
	/// Passives that the player currently have.
	/// </summary>
	private List<Items> _passives = new();

	#endregion

	#region Setters / Getters

	/// <summary>
	/// Static getter to the player reference.
	/// </summary>
	static public Player Ref { get; private set; }

	public int _XP = 0;
	public int XP
	{
		get => _XP;
		set
		{
			_XP = value;
			if (xpBar != null)
				xpBar.Value = value;
		}
	}

	private float _max_health = 100;
	public float max_health
	{
		get => _max_health;
		set
		{
			_max_health = value;
			healthBar.MaxValue = value;
		}
	}


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

	private int _level = 1;
	public int level
	{
		get => _level;
		set
		{
			_level = value;
			if (LevelLabel != null)
			{
				LevelLabel.Text = "Level: " + value; // update the level label
			}
			UI.LevelUp_Open(); // Open the level up panel when the player levels up

			if (xpBar != null)
			{
				if (value >= 7)
					xpBar.MaxValue = 40;
				else if (value >= 3)
					xpBar.MaxValue = 20;
			}
		}
	}

	private float _health = 100;
	public float health
	{
		get => _health;
		set
		{
			_health = Mathf.Max(value, 0);

			if (healthBar != null)
			{
				healthBar.Value = _health;
			}
		}
	}

	public Items Weapon { get => _weapon; }
	public List<Items> Passives { get => _passives; }

	#endregion

	#region methods
	public void take_damage(float amount) // function to reduce health
	{
		health -= Mathf.Max(amount - armor, 0);
	}

	public void _on_self_damage_body_entered(Enemy body) // this function is called everytime an enemy touch the player 
	{
		take_damage(body.Damage);
	}

	public void _on_timer_timeout()
	{
		var collision = GetNode<CollisionShape2D>("SelfDamage/Collision");
		collision.SetDeferred("disabled", true);
		collision.SetDeferred("disabled", false);
	}

	// function to gain xp
	public void Gain_XP(int amount)
	{
		XP += amount * growth;
		total_XP += amount * growth;
	}

	public void Check_XP() // function to check if the player has enough XP to level up
	{
		if (xpBar != null && XP > xpBar.MaxValue)
		{
			XP -= (int)xpBar.MaxValue;
			level += 1;
		}
	}

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

	/// <summary>
	/// Add a new passive to the player.
	/// </summary>
	/// <param name="passive">Passive to add.</param>
	public void AddPassive(Items item)
	{
		if (item.Type != Items_Type.Passive)
		{
			GD.PrintErr("AddPassive : Item isn't a passive.");
			return;
		}

		item.Holder = this;
		_passives.Add(item);

		UI.AddItemDisplay(item); // Add the passive display in the UI
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
		Check_XP();
		Animation();

		foreach (Items passive in _passives) { passive.UpdateEffectTimer(delta); }
		_weapon.UpdateEffectTimer(delta);
	}

	// used to instantiate players attributes on the game scene 
	public override void _Ready()
	{
		// Set the static getter to its reference.
		Ref = this;

		nearest_enemy_distance = 150 + area; // set nearest enemy distance to 150 + area (go in attributes to learn more)
		healthBar = GetNode<ProgressBar>("Health"); // health
		magnetArea = GetNode<CollisionShape2D>("Magnet/MagnetZone"); // magnet

		// Set a base weapon.
		_weapon = new Items(GD.Load<Weapons_Data>("res://Game/Resource/Weapons/Pistol.tres"));
		_weapon.Holder = this; // set the owner of the weapons container to this player
		UI.AddItemDisplay(_weapon); // Add the weapon display in the UI
	}

	#endregion


}
