using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Passive : Item
{
    #region attributes

    [Export]
    protected float[] _upgrades = Array.Empty<float>();

    #endregion

    #region methods

    public abstract void OnApply(PlayerControl owner);

    public virtual void EffectUpdate()
    {
        return;
    }

    #endregion
}