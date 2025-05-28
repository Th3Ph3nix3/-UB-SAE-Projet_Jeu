using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
    #region attributes

    /// <summary>
    /// Title of the item.
    /// </summary>
    [Export]
    private string _title;

    /// <summary>
    /// Texture of the item to be displayed in the UI.
    /// </summary>
    [Export]
    private Texture2D _texture;

    /// <summary>
    /// Current level of the item. Start at 1.
    /// </summary>
    private int _level = 1;

    // To change when player is better encapsulated
    protected PlayerControl _owner;

    #endregion

    #region Getter / Setter

    public Texture2D Texture
    {
        get => _texture;
    }
    public int Level
    {
        get => _level;
        set
        {
            if (value < 1)
            {
                GD.PrintErr("Level cannot be less than 1.");
                return;
            }

            _level = value;
        }
    }

    #endregion

    #region methods
    #endregion
}
