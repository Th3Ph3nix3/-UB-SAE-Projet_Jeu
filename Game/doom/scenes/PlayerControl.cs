using Godot;
using System;

public partial class PlayerControl : CharacterBody2D
{

	#region attributes
	[Export]
	public int Speed { get; set; } = 400;
	private float _health = 100;
	private ProgressBar healthBar;
	private int total_XP = 0;
	private Label LevelLabel;
	public TextureProgressBar xpBar;
	public Enemy nearest_enemy;
	public float nearest_enemy_distance = float.PositiveInfinity; // float.PositiveInfinity est la représentation de l'infini


	#endregion
	#region setters
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

	private int _level = 1;
	public int level
	{
		get => _level;
		set
		{
			_level = value;
			if (LevelLabel != null)
				LevelLabel.Text = "Lvl " + value;

			if (xpBar != null)
			{
				if (value >= 7)
					xpBar.MaxValue = 40;
				else if (value >= 3)
					xpBar.MaxValue = 20;
			}
		}
	}


	public float health
	{
		get => _health;
		set
		{
			_health = value;

			if (healthBar != null)
			{
				healthBar.Value = _health;
			}
		}
	}


	#endregion
	#region methods
	public void take_damage(float amount) // function to reduce health
	{
		health -= amount;
		GD.Print(amount); // print in godot how much damages the player received
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

	public void Gain_XP(int amount)
	{ // function to gain xp
		XP += amount;
		total_XP += amount;
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
		Velocity = inputDirection * Speed;

		if (Input.IsActionPressed("esc"))
		{
			GetTree().Quit();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (IsInstanceValid(nearest_enemy))
		{ // if the player is near an enemy
			nearest_enemy_distance = nearest_enemy.Separation;
			GD.Print(nearest_enemy.Name); // print the enemy name
		}
		else
		{
			nearest_enemy_distance = float.PositiveInfinity; // else (there is no enemy near the player), the distance is infinity
		}

		GetInput();
		MoveAndSlide();
		Check_XP();
	}

	// used to instantiate players attributes on the game scene 
	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("Health"); // health
		xpBar = GetNode<TextureProgressBar>("UI/XP"); // xp
		LevelLabel = GetNode<Label>("UI/XP/Level"); // level
	}
	#endregion
}
