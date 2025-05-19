using Godot;
using System;

public partial class PlayerControl : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;
	private float _health = 100;
	private ProgressBar healthBar;
	private int total_XP = 0;
	private Label LevelLabel;
	private Pickups area;

	
	public TextureProgressBar xpBar;
	
	public int _XP = 0;
	public int XP {
		get => _XP;
		set {
			_XP = value;
			if (xpBar != null)
				xpBar.Value = value;
		}
	}
	
	private int _level = 1;
	public int level {
		get => _level;
		set {
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

	public void Gain_XP(int amount){
		XP += amount;
		total_XP += amount;
	}
	
	public void Check_XP()
	{
		if (xpBar != null && XP > xpBar.MaxValue)
		{
			XP -= (int)xpBar.MaxValue;
			level += 1;
		}
	}

	public void _on_magnet_area_entered()
	{
		if (area.HasMethod("follow"))
		{
			area.follow((CharacterBody2D)Owner);
		}
	}

	
	public void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		Velocity = inputDirection * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(IsInstanceValid(nearest_enemy)){
			nearest_enemy_distance = nearest_enemy.separation;
			GD.Print(nearest_enemy.Name);
		}
		else{
			nearest_enemy_distance = float.PositiveInfinity;
		}

		GetInput();
		MoveAndSlide();
		Check_XP();
	}
	public override void _Ready()
	{
		healthBar = GetNode<ProgressBar>("Health");
		xpBar = GetNode<TextureProgressBar>("UI/XP");
		LevelLabel = GetNode<Label>("UI/XP/Level"); 
	}
}
