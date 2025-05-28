using Godot;
using System;

public partial class Swiftness : Passive
{
    public override void OnApply(PlayerControl owner)
    {
        _owner = owner;
        _owner.movement_speed += _upgrades[Level];
    }
}