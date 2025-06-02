using Godot;
using System;

/// <summary>
/// Base class for all passive items in the game.
/// </summary>
[GlobalClass]
public abstract partial class Passives_Data : Items_Data
{
    /// <summary>
    /// Array of upgrades available for the passive item.
    /// The first index of the list are the base stats of the passive item, and the next indices are the upgrades.
    /// </summary>
    [Export]
    public Passives_Upgrade[] upgrades = Array.Empty<Passives_Upgrade>();

    public override Base_Upgrades[] Upgrades => upgrades;

    public override Items_Type Type => Items_Type.Passive;

}