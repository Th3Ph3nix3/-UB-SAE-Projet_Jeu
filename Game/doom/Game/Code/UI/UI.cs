using System;
using System.Collections.Generic;
using Godot;

public partial class UI : CanvasLayer
{

    #region attributes

    /// <summary>
    /// Container for passive items frames in the UI.
    /// </summary>
    static private BoxContainer _passiveItemsContainer;

    /// <summary>
    /// Container for weapon items frames in the UI.
    /// </summary>
    static private BoxContainer _weaponItemsContainer;

    /// <summary>
    /// Panel that displays the level up options when the player levels up.
    /// </summary>
    static private LevelUp_Canvas _levelUp_Panel;

    #endregion

    #region Setters / Getters

    /// <summary>
    /// Gets the level up panel instance.
    /// </summary>
    public static LevelUp_Canvas LevelUp_Panel { get => _levelUp_Panel; }

    #endregion

    #region methods

    public override void _Ready()
    {
        // Initialize the level up panel.
        _levelUp_Panel = GetNode<LevelUp_Canvas>("LevelUp_Canvas");
        _levelUp_Panel.Hide(); // Hide the panel initially

        // Initialize the containers for passive and weapon items.
        _passiveItemsContainer = GetNode<BoxContainer>("PassiveItemsContainer");
        _weaponItemsContainer = GetNode<BoxContainer>("WeaponItemsContainer");

        
    }

    #region Items Display

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
            GD.PrintErr("Unknown item type: " + item.Type);
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
            GD.PrintErr("Unknown item type: " + item.Type);
        }
    }

    #endregion

    #endregion

}