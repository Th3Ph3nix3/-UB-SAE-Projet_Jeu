using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Spawner : Node2D
{

	#region attributes
	[Export]
	PlayerControl player;

	[Export]
	PackedScene enemy;

	[Export]
	private Label MinuteLabel;

	[Export]
	private Label SecondLabel;

	private float distance = 500;
	private bool can_spawn = true;

	private int enemyTypeIndex = 0;
	private int vagueCounter = 0;

	[Export]
	EnemyType[] enemy_types = new EnemyType[3];

	private int minute;
	private int second;

	#endregion
	#region setters
	public int Minute
	{
		get => minute;
		set
		{
			minute = value;
			if (MinuteLabel != null)
				MinuteLabel.Text = minute.ToString();
		}
	}

	public int Second
	{
		get => second;
		set
		{
			second = value;
			if (second >= 60)
			{
				second -= 60; 
				Minute += 1;
			}
			if (SecondLabel != null)
				SecondLabel.Text = second.ToString().PadLeft(2, '0');
		}
	}
    #endregion
    #region methods

    public override void _Ready()
    {
		InitializeEnemies();
    }

	public void InitializeEnemies()
	{
		EnemyType LittleEar = GD.Load<EnemyType>("res://Game/Resource/Enemies/Little_Ear.tres");
		EnemyType LittleMouth = GD.Load<EnemyType>("res://Game/Resource/Enemies/Little_Mouth.tres");
		EnemyType LittleEyes = GD.Load<EnemyType>("res://Game/Resource/Enemies/Little_Eyes.tres");
		
		enemy_types[0] = LittleEar;
		enemy_types[1] = LittleMouth;
		enemy_types[2] = LittleEyes;
	}


	public override void _PhysicsProcess(double _delta)
	{
		if (GetTree().GetNodeCountInGroup("Enemy") < 700) // fera spawner des mobs si le total de mobs est inférieur à 700. Changer au besoin. Fera quand meme spawn les mobs elite
		{
			can_spawn = true;
		}
		else
		{
			can_spawn = false; // mobs can't spawn to optimize memory
		}
	}

	public void spawn(Vector2 pos, bool elite = false)
	{
		if(!can_spawn && !elite)
		{
			return; // mobs can't spawn to optimize memory
		}

		Enemy enemyInstance = enemy.Instantiate() as Enemy; // creating a new enemy

		// attributes of the mob that will spawn
		enemyInstance.Type = enemy_types[enemyTypeIndex];
		enemyInstance.Position = pos;
		enemyInstance.Player_reference = player;
		enemyInstance.Elite = elite; // true or false (to have a rainbow effect)

		GetTree().CurrentScene.AddChild(enemyInstance); // add the enemy to the game scene
	}

	

	public Vector2 get_random_position()
	{
		float angle = (float)GD.RandRange(0, 2 * MathF.PI);
		return player.Position + distance * Vector2.Right.Rotated(angle);
	}

	public void amount(int number = 1)
	{
		for (int i = 0; i < number; i++)
		{
			spawn(get_random_position());
		}
	}

	public void on_timer_timeout()
	{
		Second += 1;

		if(Second >= 0 && Second <= 4)
		{
			// Update number of enemies for this wave
			enemyTypeIndex = vagueCounter % enemy_types.Length;
			vagueCounter++;

			amount((int)GD.Randi() % 15);
		}
	}

	public void _on_pattern_timeout() // For big waves. To change how much time until next, go to Godot -> Spawner.tscn -> Pattern, then in the inspector, you change Wait Time.
	{
		amount(20); // spawn 25 enemis 
	}
	public void _on_elite_timeout()
	{
		spawn(get_random_position(), true); // For Elite mob.
	}
	#endregion
}
