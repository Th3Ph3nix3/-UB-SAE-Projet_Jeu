using System;
using Godot;

/// <summary>
/// Global class to manage the player.
/// </summary>
[GlobalClass]
public partial class PlayerManager : Resource
{
    /// <summary>
    /// Hold the reference to the player.
    /// </summary>
    private PlayerControl _player;

    /// <summary>
    /// Gets the player reference, display a warning if the reference is null.
    /// </summary>
    public PlayerControl Player
    {
        get
        {
            if (_player == null)
            {
                GD.PrintErr("PlayerManager : Warning, player reference is null.");
            }

            return _player;
        }
    }

    /// <summary>
    /// Set the player reference, the reference can only be set once.
    /// </summary>
    /// <param name="player">Reference to the player.</param>
    public void SetPlayer(PlayerControl player)
    {
        if (player == null)
        {
            GD.PrintErr("PlayerManager : Can't assign player reference, reference is null.");
            return;
        }
        else if (_player != null)
        {
            GD.PrintErr("PlayerManager : Player as already been defined.");
            return;
        }

        _player = player;
    }
}
