using System;
using Godot;

/// <summary>
/// Holds all the base data of a weapon.
/// </summary>
[GlobalClass]
public abstract partial class Weapons_Data : Items_Data
{
    /// <summary>
    /// Projectile shot by the weapon.
    /// </summary>
    [Export]
    public PackedScene projectile;

    /// <summary>
    /// Upgrades available for the weapon.
    /// The first index of the list contains the base stats of the weapon, and the subsequent indices contain the upgrades.
    /// </summary>
    [Export]
    public Weapons_Upgrade[] upgrades = Array.Empty<Weapons_Upgrade>();

    public override Upgrades[] Upgrades => upgrades;

    /// <summary>
    /// Does nothing as a weapon won't have an effect on upgrade.
    /// </summary>
    public override void OnUpgrade() { return; }
}
