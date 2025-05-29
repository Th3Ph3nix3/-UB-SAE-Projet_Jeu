using Godot;
using System;

/// <summary>
/// Base class for all passive items in the game.
/// </summary>
public abstract partial class Passives : Items
{

    #region attributes

    /// <summary>
    /// Level at which the player first acquires the passive item.
    /// </summary>
    [Export]
    private int _firstOccurence;

    /// <summary>
    /// Array of upgrades available for the passive item.
    /// The first index of the list are the base stats of the passive item, and the next indices are the upgrades.
    /// </summary>
    [Export]
    protected Passives_Upgrade[] _upgrades = { new Passives_Upgrade() };

    protected override Upgrades[] Upgrades => _upgrades;

    #endregion

    #region methods

    /// <summary>
    /// Applies the passive effect to the player at each update if the passive item have one.
    /// </summary>
    protected override void EffectUpdate() { return; }

    #endregion

}