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
    private bool can_spawn = true;

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
            if (second >= 10) // à changer par la suite ! (je comprends pas trop car on a la fonction _on_pattern_timeout() mais bon)
            {
                second -= 10; // même remarque (pour que les minutes soient bien représentées)
                Minute += 1;
            }
            if (SecondLabel != null)
                SecondLabel.Text = second.ToString().PadLeft(2, '0');
        }
    }

    public override void _PhysicsProcess(double _delta)
    {
        if (GetTree().GetNodeCountInGroup("Enemy") < 700) // fera spawner des mobs si le total de mobs est inférieur à 700. Changer au besoin. Fera quand meme spawn les mobs elite
        {
            can_spawn = true;
        }
        else 
        {
            can_spawn = false;
        }
    }

    public void spawn(Vector2 pos, bool elite = false)
    {
        if(!can_spawn && !elite)
        {
            return;
        }
        Enemy enemyInstance = (Enemy)enemy.Instantiate();

        enemyInstance.Type = enemy_types[enemyTypeIndex];
        enemyInstance.Position = pos;
        enemyInstance.player_reference = player;
        enemyInstance.elite = elite;

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
        if (Second % 1 == 0) // changer au besoin le chiffre x dans (Second % x == 0). Il correspond au nombre de secondes à attendre pour qu'une vague d'ennemis arrive.
        {
            // Met à jour le type d'ennemi pour cette vague
            enemyTypeIndex = vagueCounter % enemy_types.Length;
            vagueCounter++;

            // Fais apparaître 3 ennemis d’un coup (changer au besoin)
            amount(3);
        }
    }

    public void _on_pattern_timeout() // pour les grosses vagues. Doit etre changer dans Godot -> spawner -> Pattern, puis à droite dans Inspecteur il faut changer Wait Time.
    {
        // mettre un nombre aléatoire de monstres ? -> GD.Randi() % 11 genere un nombre aléatoire entre 1 et 10
        for (int i = 0; i < GD.Randi() % 11; i++) // grâce à ça les ennemis arrivent en cercle autour du player (changer le nombre d'itération pour plus ou moins d'ennemis)
        {
            spawn(get_random_position());
        }
    }
    public void _on_elite_timeout()
    {
        spawn(get_random_position(),true); // pour le mob elite. Doit etre changer dans Godot -> spawner -> Elite, puis à droite dans Inspecteur il faut changer Wait Time.
    }

}
