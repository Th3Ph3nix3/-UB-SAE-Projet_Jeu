using Godot;
using System;
using System.Drawing;
using System.Numerics;

public partial class Enemy : CharacterBody2D
{
	#region  Attributes

	/// <summary>
	/// Type of the enemy.
	/// </summary>
	private EnemyType _type;
	private AudioStreamPlayer soundMonster;
	private Timer soundTimer;
	private RandomNumberGenerator rng = new RandomNumberGenerator();

	// Set your min and max delay (in seconds)
	[Export] public float MinDelay = 1.0f;
	[Export] public float MaxDelay = 3.0f;

	#region Base stats

	/// <summary>
	/// Health of the enemy. If it reaches 0, the enemy is removed from the scene.
	/// </summary>
	private float _health;

	/// <summary>
	/// Speed of the enemy.
	/// </summary>
	private float _speed;

	/// <summary>
	/// Damage of the enemy.
	/// </summary>
	private float _damage;

	#endregion

	/// <summary>
	/// True if the enemy is an elite enemy, false otherwise.
	/// </summary>
	private bool _elite = false;

	#region Displacement

	/// <summary>
	/// Current direction and force of the enemy.
	/// </summary>
	private Godot.Vector2 _direction;

	/// <summary>
	/// Current direction and force of the knockback applied to this enemy.
	/// </summary>
	private Godot.Vector2 _knockback;

	/// <summary>
	/// Distance between the enemy and the player.
	/// </summary>
	private float _separation;

	#endregion

	#region animation

	/// <summary>
	/// Time since the last animation frame.
	/// </summary>
	private float _duration = 0;

	/// <summary>
	/// Frames per second of the animation.
	/// </summary>
	private int _fps = 10;

	#endregion

	#region Node/Scene

	/// <summary>
	/// Reference to the player it chase.
	/// </summary>
	[Export]
	private CharacterBody2D _player_reference;

	/// <summary>
	/// Link the sprite2D to the enemy.
	/// </summary>
	[Export]
	private Sprite2D _sprite2D;

	/// <summary>
	/// Link the collision shape to the enemy.
	/// </summary>
	[Export]
	private CollisionShape2D _collisionShape2D;

	/// <summary>
	/// Load the scene of the damage popup.
	/// </summary>
	private PackedScene _damage_popup_node = GD.Load<PackedScene>("res://scenes/damage.tscn");

	/// <summary>
	/// Load the scene of the pickups items.
	/// </summary>
	private PackedScene _dropScene = GD.Load<PackedScene>("res://scenes/pickups.tscn");

	#endregion

	#endregion

	#region Setters / Getters

	public EnemyType Type
	{
		get => _type;
		set
		{
			_type = value;
			_damage = value.damage;
			_health = value.health;
			_speed = value.speed;
		}
	}
	/// <summary>
	/// Health of the enemy. If it reaches 0, the enemy is removed from the scene.
	/// </summary>
	public float Health
	{
		get => _health;
		set
		{
			_health = value;
			if (_health <= 0)
			{
				DropItem();
				QueueFree();
			}
		}
	}
	public float Speed
	{
		get => _speed;
	}
	public float Damage
	{
		get => _damage;
	}
	public bool Elite
	{
		get => _elite;
		set
		{
			_elite = value;
		}
	}
	public Godot.Vector2 Knockback
	{
		get => _knockback;
		set => _knockback = value;
	}
	public float Separation
	{
		get => _separation;
		set => _separation = value;
	}
	public CharacterBody2D Player_reference
	{
		get => _player_reference;
		set => _player_reference = value;
	}

	#endregion

	#region Ready and Physics Process

	/// <summary>
	/// Called when the enemy is added to the scene.
	/// Link the sprite2D at this time and set the texture frames and shaders.
	/// </summary>
	public override void _Ready()
	{
		_sprite2D = GetNode<Sprite2D>("Sprite2D");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

		if (_sprite2D == null && _collisionShape2D == null && _type == null)
		{
			QueueFree(); // If the sprite2D, CollisionShape2D or type is not found, remove the enemy from the scene.
		}

		_sprite2D.Texture = _type.texture;
		_sprite2D.Hframes = _type.frames;

		//get the caracteristics of a non-elite mob
		EnemyType NormalType = this._type;

		if (!_elite)
		{
			// Set the shader for the sprite2D.
			// _sprite2D.Material = GD.Load<ShaderMaterial>("res://Shaders/matOutline.tres");

			// Scale the enemy accordingly.
			_sprite2D.Scale = new Godot.Vector2(2.5f, 2.5f);
			_collisionShape2D.Scale = new Godot.Vector2(2.5f, 2.5f);

		}
		else
		{
			// Set the shader for the sprite2D.
			_sprite2D.Material = GD.Load<ShaderMaterial>("res://Shaders/Rainbow.tres");

			// Scale the enemy accordingly.
			_sprite2D.Scale = new Godot.Vector2(5f, 5f);
			_collisionShape2D.Scale = new Godot.Vector2(5f, 5f);
		}

		soundMonster = GetNode<AudioStreamPlayer>("AudioStreamPlayer2D");
		soundTimer = GetNode<Timer>("Timer");

		soundTimer.Timeout += OnSoundTimerTimeout;
		StartRandomSoundLoop();
	}

	/// <summary>
	/// Each frame, update the velocity of the enemy to move toward the player,
	/// the animation of the enemy, the separation between the enemy and the player and
	/// the knockback state.
	/// </summary>
	public override void _PhysicsProcess(double delta)
	{
		// Update the mouvement of the enemy.
		Velocity = (Player_reference.Position - Position).Normalized() * _speed;

		Animation(delta);
		check_separation(delta);
		knockback_update(delta);
	}
	#endregion

	#region Methods

	/// <summary>
	/// Manage the sprite : animation of the enemy and the sprite flipping.
	/// </summary>
	public void Animation(double delta)
	{
		// If the enemy is in a knockback state, it doesn't flip the sprite.
		if (Knockback == Godot.Vector2.Zero)
		{
			if (Velocity.X > 0)
			{
				_sprite2D.FlipH = true;
			}
			else if (Velocity.X < 0)
			{
				_sprite2D.FlipH = false;
			}
		}

		// If there isn't any animation, return.
		if (_type.frames <= 1)
		{
			return;
		}

		_duration += (float)delta;

		if (_type.frames > 1 && _duration >= 1f / _fps)
		{
			_sprite2D.Frame = (_sprite2D.Frame + 1) % _type.frames;
			_duration = 0;
		}
	}

	/// <summary>
	/// Check the distance between the enemy and the player.
	/// Used to update _separation each frame.
	/// </summary>
	public void check_separation(double _delta)
	{
		Separation = (Player_reference.Position - Position).Length();
		if (Separation >= 2000 && !Elite) // if the mob is not elite and is too far of the player
		{
			QueueFree(); // free memory by destroying the mob
		}

		PlayerControl player = Player_reference as PlayerControl; // cast player_reference

		if (Separation < player.nearest_enemy_distance) // updating nearest_enemy of player
		{
			player.nearest_enemy_distance = Separation;
			player.nearest_enemy = this;
		}
	}

	/// <summary>
	/// Manage the knockack that can be applied to an enemy.
	/// Also manage the collision between enemies that result from a knockback.
	/// </summary>
	public void knockback_update(double delta)
	{
		Knockback = MoveToward(Knockback, Godot.Vector2.Zero, 1);
		Velocity += Knockback;

		var collider = MoveAndCollide(Velocity * (float)delta);
		if (collider != null)
		{
			var hitNode = collider.GetCollider();
			var hitEnemy = hitNode as Enemy;
			if (hitEnemy != null)
			{
				var hitEnemyPosition = hitEnemy.GlobalPosition;
				hitEnemy.Knockback = (hitEnemyPosition - GlobalPosition).Normalized() * 50;
			}
		}
	}

	/// <summary>
	/// Allow to move an object toward a target position by a max distance per update.
	/// </summary>
	/// <param name="current">Current position of the object to move</param>
	/// <param name="target">Current position of the target object</param>
	/// <param name="maxDistanceDelta">The longest distance makable in this update</param>
	/// <returns>The new position of the object to move</returns>
	public Godot.Vector2 MoveToward(Godot.Vector2 current, Godot.Vector2 target, float maxDistanceDelta)
	{
		Godot.Vector2 direction = target - current;
		float distance = direction.Length();

		if (distance <= maxDistanceDelta || distance == 0)
		{
			return target;
		}

		direction = direction.Normalized();

		return current + direction * maxDistanceDelta;
	}

	/// <summary>
	/// Display a damage popup when the enemy is hit.
	/// </summary>
	/// <param name="amount">Amount of damage</param>
	public void damage_popup(float amount)
	{
		var popup = _damage_popup_node.Instantiate<Damage>();
		popup.Text = amount.ToString();
		popup.Position = Position + new Godot.Vector2(-50, -50);
		GetTree().CurrentScene.AddChild(popup);
	}

	/// <summary>
	/// Manage the behavior of the enemy when it is hit.
	/// </summary>
	/// <param name="amount">Amount of damage</param>
	public void take_damage(float amount)
	{
		// Make the enemy flash when it is hit.
		var tween = GetTree().CreateTween();
		tween.TweenProperty(_sprite2D, "modulate", new Godot.Color(3, (float)0.25, (float)0.25), 0.1);
		tween.Chain().TweenProperty(_sprite2D, "modulate", new Godot.Color(1, 1, 1), 0.1);
		tween.BindNode(this);

		damage_popup(amount);
		Health -= amount;
	}

	/// <summary>
	/// Called when an enemy is killed.
	/// Manage the drop of items.
	/// </summary>
	public void DropItem()
	{
		// Return if there's nothing to drop.
		if (_type.drops.Length == 0)
		{
			return;
		}

		var random = new Random();
		int index = random.Next(_type.drops.Length);
		var item = _type.drops[index];

		var itemToDrop = _dropScene.Instantiate<Pickups>();

		itemToDrop.type = item;
		itemToDrop.Position = Position;
		itemToDrop.player_reference = Player_reference;

		GetTree().CurrentScene.CallDeferred("add_child", itemToDrop); // add to the scene tree
	}
	private void StartRandomSoundLoop()
    {
        SetRandomTimer();
        soundTimer.Start();
    }

    private void OnSoundTimerTimeout()
    {
        soundMonster.Play();
        SetRandomTimer();
        soundTimer.Start();
    }

    private void SetRandomTimer()
    {
        float delay = rng.RandfRange(MinDelay, MaxDelay);
        soundTimer.WaitTime = delay;
    }

	#endregion
}
