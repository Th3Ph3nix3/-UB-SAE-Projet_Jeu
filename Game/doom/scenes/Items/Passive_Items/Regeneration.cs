using Godot;
using System;

/// <summary>
/// Represents a passive item that regenerates the player's health over time.
/// </summary>
[GlobalClass]
public partial class Regeneration : Passives
{
    /// <summary>
    /// Applies the regeneration effect to the player by increasing their health.
    /// </summary>
    protected override void EffectUpdate()
	{
		_owner.health += _upgrades[Level].stat;
	}
}
