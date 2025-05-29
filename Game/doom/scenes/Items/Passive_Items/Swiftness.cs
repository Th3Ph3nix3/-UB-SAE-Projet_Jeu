using Godot;
using System;

/// <summary>
/// Represents a passive item that increases the player's movement speed.
/// </summary>
[GlobalClass]
public partial class Swiftness : Passives
{
    protected override void OnUpgrade()
    {
        if (Level == 0)
        {
            _owner.movement_speed = _upgrades[Level].stat; // Set initial movement speed
        }
        else
        {
            _owner.movement_speed += _upgrades[Level].stat - _upgrades[Level - 1].stat; // Increase movement speed by the difference between current and previous upgrade
        }
    }
}