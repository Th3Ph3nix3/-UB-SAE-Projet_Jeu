using System;
using System.Collections.Generic;
using Godot;

public partial class UI : CanvasLayer
{

    #region methods

    public override void _Ready()
    {
        // Initialize the containers for passive and weapon items.
        _passiveItemsContainer = GetNode<BoxContainer>("PassiveItemsContainer");
        _weaponItemsContainer = GetNode<BoxContainer>("WeaponItemsContainer");

        // Initialize the level up panel.
        _levelUp_Canvas = GetNode<LevelUp_Canvas>("LevelUp_Canvas");
    }

    #region Player UI

    #region Items Display

    /// <summary>
    /// Container for passive items frames in the UI.
    /// </summary>
    static private BoxContainer _passiveItemsContainer;

    /// <summary>
    /// Container for weapon items frames in the UI.
    /// </summary>
    static private BoxContainer _weaponItemsContainer;

    /// <summary>
    /// Adds an item frame to the UI based on the type of item.
    /// </summary>
    /// <param name="item">Item to add.</param>
    static public void AddItemDisplay(Items item)
    {
        Items_Frame itemFrame = Items_Frame.new_Items_Frame(item);

        if (item.Type == Items_Type.Passive)
        {
            _passiveItemsContainer.AddChild(itemFrame);
        }
        else if (item.Type == Items_Type.Weapon)
        {
            _weaponItemsContainer.AddChild(itemFrame);
        }
        else
        {
            GD.PrintErr("UI : Unknown item type: " + item.Type);
        }
    }

    /// <summary>
    /// Update a item frame in the UI based on the item.
    /// </summary>
    static public void UpdateItemsDisplay(Items item)
    {
        if (item.Type == Items_Type.Passive)
        {
            foreach (Items_Frame itemsFrame in _passiveItemsContainer.GetChildren())
            {
                if (itemsFrame.Item == item)
                {
                    itemsFrame.UpdateFrame();
                    return;
                }
            }
        }
        else if (item.Type == Items_Type.Weapon)
        {
            foreach (Items_Frame itemsFrame in _weaponItemsContainer.GetChildren())
            {
                if (itemsFrame.Item == item)
                {
                    itemsFrame.UpdateFrame();
                    return;
                }
            }
        }
        else
        {
            GD.PrintErr("UI : Unknown item type: " + item.Type);
        }
    }

    #endregion

    #region Player Info

    /// <summary>
    /// Display the player's current level.
    /// </summary>
    private Label LevelLabel;

    /// <summary>
    /// Displays the player's xp as a progress bar.
    /// </summary>
	private TextureProgressBar xpBar;

    #endregion

    #endregion

    #region Level Up Canvas

    /// <summary>
    /// Panel that displays the level up options when the player levels up.
    /// </summary>
    static private LevelUp_Canvas _levelUp_Canvas;

    /// <summary>
    /// Opens the Level Up panel to allow the player to choose upgrades after leveling up.
    /// If the panel is not initialized, it will print an error message.
    /// </summary>
    static public void LevelUp_Open()
    {
        if (_levelUp_Canvas != null)
        {
            _levelUp_Canvas.Open();
        }
        else
        {
            GD.PrintErr("UI : LevelUp_Canvas is not initialized.");
        }
    }

    /// <summary>
    /// Closes the Level Up panel.
    /// If the panel is not initialized, it will print an error message.
    /// </summary>
    static public void LevelUp_Close()
    {
        if (_levelUp_Canvas != null)
        {
            _levelUp_Canvas.Close();
        }
        else
        {
            GD.PrintErr("UI : LevelUp_Canvas is not initialized.");
        }
    }

    #endregion

    #endregion

}