using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

    Vector2 direction;
    float speed = 75;

    [Export]
    public CharacterBody2D player_reference;

    public override void _PhysicsProcess(double delta){
        Velocity = (player_reference.Position - Position).Normalized() * speed;
        MoveAndCollide(Velocity * (float)delta);
    }
}
