using System;
using Godot;

/// <summary>
/// Database containing references to all items and the player.
/// </summary>
[GlobalClass]
public partial class Database : Resource
{

    /// <summary>
    /// Array of all passive items in the game.
    /// </summary>
    [Export]
    private Passives[] _passivesList = Array.Empty<Passives>();

    /// <summary>
    /// Array of all weapons in the game.
    /// </summary>
    [Export]
    private Weapons[] _weaponsList = Array.Empty<Weapons>();

    /// <summary>
    /// Gets the array of all passive items in the game.
    /// </summary>
    public Passives[] PassivesList
    {
        get => _passivesList;
    }

    /// <summary>
    /// Gets the array of all weapons in the game.
    /// </summary>
    public Weapons[] WeaponsList
    {
        get => _weaponsList;
    }
}
