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
    private int vagueCounter = 0;



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

        enemyInstance.Type = enemy_types[enemyTypeIndex];
        enemyInstance.Position = pos;
        enemyInstance.player_reference = player;

        GetTree().CurrentScene.AddChild(enemyInstance);
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

        // Toutes les 10 secondes : nouvelle vague
        // attention, les 10 premières secondes, personne ne spawn car le nb de secondes n'est pas divisible par 10
        if (Second % 10 == 0) // changer au besoin le chiffre x dans (Second % x == 0). Il correspond au nombre de secondes à attendre pour qu'une vague d'ennemis arrive.
        {
            // Met à jour le type d'ennemi pour cette vague
            enemyTypeIndex = vagueCounter % enemy_types.Length;
            vagueCounter++;

            // Fais apparaître 3 ennemis d’un coup (changer au besoin)
            amount(3);
        }
    }

}
