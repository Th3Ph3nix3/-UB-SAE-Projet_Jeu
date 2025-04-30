using Godot;
using System;
using System.Collections.Generic;

public partial class Spawner : Node2D
{
    [Export]
    CharacterBody2D player;

    [Export]
    PackedScene enemy;

    [Export]
    private Label MinuteLabel;

    [Export]
    private Label SecondLabel;

    private float distance = 400;

    private int enemyTypeIndex = 0;


    [Export]
    EnemyType[] enemy_types;



    private int minute;
    private int second;

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
            if (second >= 10) // à mettre à 60 par la suite !
            {
                second -= 10; // même remarque (pour que les minutes soient bien représentées)
                Minute += 1;
            }
            if (SecondLabel != null)
                SecondLabel.Text = second.ToString().PadLeft(2, '0');
        }
    }

    public void spawn(Vector2 pos)
    { 
        Enemy enemyInstance = (Enemy)enemy.Instantiate();
        
        enemyInstance.Type = enemy_types[Math.Min(enemyTypeIndex, enemy_types.Length - 1)];

        enemyInstance.Position = pos;

        enemyInstance.player_reference = player;

        GetTree().CurrentScene.AddChild(enemyInstance);

        enemyTypeIndex = (enemyTypeIndex + 1) % enemy_types.Length;
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
		amount(second % 10);
	}
}
