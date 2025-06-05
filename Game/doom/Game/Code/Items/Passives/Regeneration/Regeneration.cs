using Godot;
using System;

/// <summary>
/// Represents a passive item that regenerates the player's health over time.
/// </summary>
[GlobalClass]
public partial class Regeneration : Passives_Data
{
    /// <summary>
    /// Applies the regeneration effect to the player by increasing their health.
    /// </summary>
    public override void EffectUpdate(int level)
    {
        holder.Health += upgrades[level].stat;
    }

    /// <summary>
    /// This passives doesn't have an effect on upgrade.
    /// </summary>
    public override void OnUpgrade(int level) { return; }

}
