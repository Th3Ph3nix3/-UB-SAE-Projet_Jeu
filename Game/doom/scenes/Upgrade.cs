using Godot;
using System;

[GlobalClass]
public partial class Upgrade : Resource
{
    #region Attributes
    
    [Export]
    public float damage;

    [Export]
    public float cooldown;

    [Export(PropertyHint.MultilineText)]
    public string description;
    #endregion
}
