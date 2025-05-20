using Godot;
using System;
using System.Drawing;
using System.Numerics;

public partial class Enemy : CharacterBody2D
{
	#region  attributes
	Godot.Vector2 direction;
	public float speed = 75;
	public float damage;
	public Godot.Vector2 knockback;
	public float separation;

	private AnimationPlayer _animationPlayer;

	[Export]
	public CharacterBody2D player_reference;

	[Export]
	public Sprite2D Sprite2D;

	public PackedScene damage_popup_node = GD.Load<PackedScene>("res://scenes/damage.tscn");
	// 1) charge la scène
	private PackedScene dropScene = GD.Load<PackedScene>("res://scenes/pickups.tscn");


	#endregion

	#region setters

	public float _health;

	public float health
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


	public bool _elite = false;

	public bool elite
	{
		get => _elite;
		set
		{
			_elite = value;
			if (Sprite2D != null && value)  // cf ligne 57
			{
				var mat = GD.Load<ShaderMaterial>("res://Shaders/Rainbow.tres");
				Sprite2D.Material = mat;
				Scale = new Godot.Vector2(5f, 5f);
			}
		}
	}



	private EnemyType type;

	public EnemyType Type
	{
		get => type;
		set
		{
			type = value;
			UpdateSpriteTexture();
			damage = value.damage;
			health = value.health;
		}
	}

	#endregion

	#region methods

	private void UpdateSpriteTexture()
	{
		if (Sprite2D != null && type != null)
			Sprite2D.Texture = type.texture;
	}

	public override void _Ready()
	{
		Sprite2D = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("Ear_Walk");
		//UpdateSpriteTexture();

		if (_elite && Sprite2D.Material == null) // if the mob is elite
		{
			var mat = GD.Load<ShaderMaterial>("res://Shaders/Rainbow.tres"); // call the rainbow effect
			Sprite2D.Material = mat;
			Sprite2D.Scale = new Godot.Vector2(5f, 5f);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		check_separation(delta);
		knockback_update(delta);

		if (Velocity.X > 0)
		{
			Sprite2D.FlipH = true;
		}
		else if (Velocity.X < 0)
		{
			Sprite2D.FlipH = false;
		}
	}

	public void check_separation(double _delta)
	{
		separation = (player_reference.Position - Position).Length();
		if (separation >= 500 && !elite) // if the mob is not elite and is too far of the player
		{
			QueueFree(); // free memory by destroying the mob
		}

		var player = player_reference as PlayerControl; // cast player_reference

		if (separation < player.nearest_enemy_distance) // updating nearest_enemy
		{
			player.nearest_enemy_distance = separation;
			player.nearest_enemy = this;
		}
	}

	public void knockback_update(double delta) // to make a knockback
	{
		Godot.Vector2 targetPosition = player_reference.Position;
		Godot.Vector2 moveDirection = targetPosition - Position;
		moveDirection = moveDirection.Normalized();

		Velocity = moveDirection * speed;

		knockback = MoveToward(knockback, Godot.Vector2.Zero, 1);
		Velocity += knockback;

		var collider = MoveAndCollide(Velocity * (float)delta);
		if (collider != null)
		{
			var hitNode = collider.GetCollider();
			var hitEnemy = hitNode as Enemy;
			if (hitEnemy != null)
			{
				var hitEnemyPosition = hitEnemy.GlobalPosition;
				hitEnemy.knockback = (hitEnemyPosition - GlobalPosition).Normalized() * 50;
			}
		}
	}

	// reproduction du fonctionnement de MoveToward parce que elle n'est pas implémentée en CS
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

	// function to instantiate damage popup & add it to the scene
	public void damage_popup(float amount)
	{
		var popup = damage_popup_node.Instantiate<Damage>();
		popup.Text = amount.ToString();
		popup.Position = Position + new Godot.Vector2(-50, -50);
		GetTree().CurrentScene.AddChild(popup);
	}

	// whenever enemy is hit by a projectile
	public void take_damage(float amount)
	{
		var tween = GetTree().CreateTween();
		tween.TweenProperty(Sprite2D, "modulate", new Godot.Color(3, (float)0.25, (float)0.25), 0.1);
		tween.Chain().TweenProperty(Sprite2D, "modulate", new Godot.Color(1, 1, 1), 0.1);
		tween.BindNode(this);

		damage_popup(amount);
		health -= amount;
	}


	public void DropItem() // function called when an enemy died
	{
		if (type.drops.Length == 0)
		{
			return;
		}


		var random = new Random();
		int index = random.Next(type.drops.Length);
		var item = type.drops[index];

		var itemToDrop = dropScene.Instantiate<Pickups>();

		itemToDrop.type = item;
		itemToDrop.Position = Position;
		itemToDrop.player_reference = player_reference;

		GetTree().CurrentScene.CallDeferred("add_child", itemToDrop); // add to the scene tree
	}

	#endregion
}
