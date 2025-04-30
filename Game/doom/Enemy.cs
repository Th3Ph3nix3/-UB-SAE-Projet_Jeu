using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    Vector2 direction;
    float speed = 75;

    [Export]
    public CharacterBody2D player_reference;

    [Export]
    public Sprite2D Sprite2D { get; set; }

    private EnemyType type;

    public EnemyType Type
    {
        get => type;
        set
        {
            type = value;
            UpdateSpriteTexture();
        }
    }

    private void UpdateSpriteTexture()
    {
        if (Sprite2D != null && type != null)
            Sprite2D.Texture = type.texture;
    }

    public override void _Ready()
    {
        UpdateSpriteTexture();
    }


    public override void _PhysicsProcess(double delta)
    {
        Velocity = (player_reference.Position - Position).Normalized() * speed;
        MoveAndCollide(Velocity * (float)delta);
    }
}
