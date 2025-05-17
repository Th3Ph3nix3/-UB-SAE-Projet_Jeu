using Godot;
using System;

public partial class PlayerControl : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	private float _health = 100;
	private ProgressBar healthBar;

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

	public Enemy nearest_enemy; // peut etre changer le type en Enemy ou en CharacterBody2D
	public float nearest_enemy_distance = float.PositiveInfinity; // float.PositiveInfinity est la représentation de l'infini

	public void take_damage(float amount) // function to reduce health
	{
		health -= amount;
		GD.Print(amount);
	}
	
	public void _on_self_damage_body_entered(Enemy body)
	{
		take_damage(body.damage);
	}

	public void _on_timer_timeout()
	{
		var collision = GetNode<CollisionShape2D>("SelfDamage/Collision"); 
		collision.SetDeferred("disabled", true);
		collision.SetDeferred("disabled", false);
	}

	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(nearest_enemy != null){
			nearest_enemy_distance = nearest_enemy.separation;
			GD.Print(nearest_enemy.Name);
		}
		else{
			nearest_enemy_distance = float.PositiveInfinity;
		}

		GetInput();
		MoveAndSlide();
	}
	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("Health");
	}
}
