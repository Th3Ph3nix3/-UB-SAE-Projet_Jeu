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
    public static Database Database { get; private set; }

    /// <summary>
    /// Reference to the player.
    /// </summary>
    [Export]
    private PlayerControl _player;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        Database = GD.Load<Database>("res://scenes/Global/Database.tres");
    }

    /// <summary>
    /// Gets the reference to the player.
    /// </summary>
    public PlayerControl Player
    {
        get => _player;
    }
}
