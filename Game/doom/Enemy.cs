using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    Vector2 direction;
    public float speed = 75;
    public float damage;

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
                Scale = new Vector2(5f,5f);
            }
		}
	}

    [Export]
    public CharacterBody2D player_reference;

    [Export]
    public Sprite2D Sprite2D;

    private EnemyType type;

    public EnemyType Type
    {
        get => type;
        set
        {
            type = value;
            UpdateSpriteTexture();
            damage = value.damage;
        }
    }

    private void UpdateSpriteTexture()
    {
        if (Sprite2D != null && type != null)
            Sprite2D.Texture = type.texture;
    }

    public override void _Ready()
    {
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        UpdateSpriteTexture();

        if (_elite && Sprite2D.Material == null) // cf ligne 18
        {
            var mat = GD.Load<ShaderMaterial>("res://Shaders/Rainbow.tres");
            Sprite2D.Material = mat;
            Sprite2D.Scale = new Vector2(5f, 5f);
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        Velocity = (player_reference.Position - Position).Normalized() * speed;
        MoveAndCollide(Velocity * (float)delta);
    }
}
