using Godot;
using System;

public partial class Pickups : Area2D
{
    #region attributes
    public bool can_follow;
    #endregion
    #region methods
    public void follow(CharacterBody2D _target)
    {
        can_follow = true;
    }
    #endregion
}
