using Godot;
using System;

/// <summary>
/// Represents a passive item that increases the player's movement speed.
/// </summary>
[GlobalClass]
public partial class Swiftness : Passives_Data
{
    /// <summary>
    /// This passive doesn't have an continuous effect.
    /// </summary>
    public override void EffectUpdate() { return; }

    /// <summary>
    /// When the passive is equipped or when it's upgraded, it add it's current level value to the player movement speed.
    /// </summary>
    public override void OnUpgrade()
    {
        if (level == 0)
        {
            holder.movement_speed = upgrades[level].stat; // Set initial movement speed
        }
        else
        {
            holder.movement_speed += upgrades[level].stat - upgrades[level - 1].stat; // Increase movement speed by the difference between current and previous upgrade
        }
    }
}