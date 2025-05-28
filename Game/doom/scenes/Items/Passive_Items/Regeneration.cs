using Godot;
using System;

public partial class Regeneration : Passive
{
    public override void OnApply(PlayerControl owner)
    {
        _owner = owner;
    }

    public override void EffectUpdate()
    {
        _owner.health += _upgrades[Level];
    }
}