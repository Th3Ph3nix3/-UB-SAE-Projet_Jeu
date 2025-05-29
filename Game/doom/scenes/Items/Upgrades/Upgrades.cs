using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

/// <summary>
/// Base class holding the attributes for item upgrades.
/// </summary>
public abstract partial class Upgrades : Resource
{

    /// <summary>
    /// Cooldown of the item at a given level. (in seconds)
    /// </summary>
    [Export]
    public double cooldown = 0;

    /// <summary>
    /// Text description of the upgrade, displayed in the UI.
    /// </summary>
    [Export(PropertyHint.MultilineText)]
    private string description;

}

