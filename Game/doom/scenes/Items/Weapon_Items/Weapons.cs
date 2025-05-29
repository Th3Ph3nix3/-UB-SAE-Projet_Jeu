using System;
using Godot;

/// <summary>
/// Base class for all weapons in the game.
/// </summary>
public abstract partial class Weapons : Items
{

    #region attributes

    /// <summary>
    /// Projectile shot by the weapon.
    /// </summary>
    [Export]
    protected PackedScene _projectile_node;

    /// <summary>
    /// Upgrades available for the weapon.
    /// The first index of the list contains the base stats of the weapon, and the subsequent indices contain the upgrades.
    /// </summary>
    [Export]
    protected Weapons_Upgrade[] _upgrades = { new Weapons_Upgrade() };

    protected override Upgrades[] Upgrades => _upgrades;

    #endregion

    #region methods

    /// <summary>
    /// Shoot method to be implemented by each weapon.
    /// </summary>
    protected abstract void Shoot();

    /// <summary>
    /// Update method for the weapon's effect.
    /// </summary>
    protected override void EffectUpdate()
    {
        Shoot();
    }

    #endregion

}
