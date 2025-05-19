using Godot;
using System;

public partial class Pickups : Area2D
{

    public bool can_follow;
    public void follow(CharacterBody2D _target)
    {
        can_follow = true;
    }
}
