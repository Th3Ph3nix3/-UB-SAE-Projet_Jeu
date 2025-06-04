using System;
using Godot;

/// <summary>
/// Base class holding the attributes for passive item upgrades.
/// </summary>
[GlobalClass]
public partial class Passives_Upgrade : Base_Upgrades
{

    /// <summary>
    /// Stat of the passive item at a given level. This could represent various attributes such as health, speed, or damage.
    /// </summary>
    [Export]
    public int stat = 0;

}
