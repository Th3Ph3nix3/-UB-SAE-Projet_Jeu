using System;
using Godot;
using System.Collections.Generic;
using System.IO;


/// <summary>
/// Database containing references to all items and the player.
/// </summary>
public class Database
{
    /// <summary>
    /// Array of all passive items in the game.
    /// </summary>
    private List<Passives_Data> _passivesList = new();

    /// <summary>
    /// Array of all weapons in the game.
    /// </summary>
    private List<Weapons_Data> _weaponsList = new();

    /// <summary>
    /// Gets the array of all passive items in the game.
    /// </summary>
    public List<Passives_Data> PassivesList
    {
        get => _passivesList;
    }

    /// <summary>
    /// Gets the array of all weapons in the game.
    /// </summary>
    public List<Weapons_Data> WeaponsList
    {
        get => _weaponsList;
    }

    public Database()
    {
        foreach (string file in Directory.GetFiles("Game/Resource/Passives/"))
        {
            if (Path.GetExtension(file) == ".tres")
            {
                _passivesList.Add(GD.Load<Passives_Data>(file));
            }
        }

        foreach (string file in Directory.GetFiles("Game/Resource/Weapons/"))
        {
            if (Path.GetExtension(file) == ".tres")
            {
                _weaponsList.Add(GD.Load<Weapons_Data>(file));
            }
        }
    }
}
