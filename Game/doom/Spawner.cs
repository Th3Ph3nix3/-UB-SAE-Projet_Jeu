using Godot;
using System;

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
            if (second >= 10)
            {
                second -= 10;
                minute += 1;
            }
            if (SecondLabel != null)
                SecondLabel.Text = second.ToString().PadLeft(2, '0');
        }
    }

	private float timerAccumulator = 0f;

	public override void _Process(double delta)
	{
		timerAccumulator += (float)delta;

		if (timerAccumulator >= 1f)
		{
			timerAccumulator -= 1f;
			Second += 1;
		}
	}


    public void spawn(Vector2 pos)
    {
        Enemy enemyInstance = (Enemy)enemy.Instantiate();
        enemyInstance.Position= pos;
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

	public void _on_timer_timeout()
	{
		Second += 1;
		amount(second % 10);
	}
}
