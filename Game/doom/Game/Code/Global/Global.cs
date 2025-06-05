using System;
using Godot;

/// <summary>
/// Global singleton autoloaded in the game holding the database.
/// </summary>
public partial class Global : Node
{
    /// <summary>
	/// Number of xp more the player have to get to level up at each passing level
	/// </summary>
	static public readonly int XP_SCALE = 5;

    /// <summary>
    /// Minimum xp the player have to get to level up.
    /// </summary>
    static public readonly int BASE_XP_TO_GET = 5;

    /// <summary>
    /// Interval of level between the possibily to add a new passive item.
    /// </summary>
    static public readonly int NEW_PASSIVE_INTERVAL = 2;

    /// <summary>
    /// Base max player health.
    /// </summary>
    static public readonly int BASE_MAX_PLAYER_HEALTH = 100;
}
