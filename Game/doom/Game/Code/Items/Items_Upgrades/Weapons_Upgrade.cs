using System;
using Godot;

/// <summary>
/// Base class for weapon upgrades, defining attributes such as damage, cooldown, speed, and description.
/// </summary>
[GlobalClass]
public partial class Weapons_Upgrade : Base_Upgrades
{
    /// <summary>
    /// Damage of the weapon at its current level.
    /// </summary>
    [Export]
    public int damage = 0;

    /// <summary>
    /// Speed of the projectile of the weapon at its current level.
    /// </summary>
    [Export]
    public int speed = 0;

}
