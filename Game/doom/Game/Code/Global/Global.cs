using System;
using Godot;

/// <summary>
/// Global singleton autoloaded in the game holding the database.
/// </summary>
public partial class Global : Node
{
    /// <summary>
    /// Reference to the database containing all items and the player.
    /// </summary>
    static public Database Database { get; private set; }

    /// <summary>
    /// Reference to the player manager.
    /// </summary>
    static public PlayerManager PlayerManager { get; private set; }

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        Database = new Database();
        PlayerManager = new PlayerManager();
    }
}
