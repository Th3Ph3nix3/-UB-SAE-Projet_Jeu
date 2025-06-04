using System;
using System.Collections.Generic;
using Godot;

public partial class PlayerInfo_Canvas : CanvasLayer
{

    #region attributes

    /// <summary>
    /// Progress bar that display the quantity of Xp gained.
    /// </summary>
    private TextureProgressBar _xpBar;

    /// <summary>
    /// Label displaying the current level of the player.
    /// </summary>
    private Label _levelLabel;

    /// <summary>
    /// Progress bar that display the current quantity of health.
    /// </summary>
    private TextureProgressBar _healthBar;

    /// <summary>
    /// Container for passive items frames in the UI.
    /// </summary>
    private BoxContainer _passiveItemsContainer;

    /// <summary>
    /// Container for weapon items frames in the UI.
    /// </summary>
    private BoxContainer _weaponItemsContainer;

    #endregion

    #region methods

    public override void _Ready()
    {
        _xpBar = GetNode<TextureProgressBar>("XP");
        _levelLabel = GetNode<Label>("XP/Level");
        _healthBar = GetNode<TextureProgressBar>("Health");

        _passiveItemsContainer = GetNode<BoxContainer>("PassiveItemsContainer");
        _weaponItemsContainer = GetNode<BoxContainer>("WeaponItemsContainer");
    }

    /// <summary>
    /// Adds an item frame to the UI based on the type of item.
    /// </summary>
    /// <param name="item">Item to add.</param>
    public void AddItemDisplay(Items item)
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
    /// Update the xp bar to display the amount of xp the player has.
    /// </summary>
    /// <param name="currentXp">Xp that the player have currently.</param>
    public void UpdateXpBar(int currentXp)
    {
        _xpBar.Value = currentXp;
    }

    /// <summary>
    /// Update the player level and set the new amount of xp to get when the player level up.
    /// </summary>
    /// <param name="currentLevel">Current level of the player.</param>
    /// <param name="maxXp">Current amount of xp the player has to get to level up.</param>
    public void LevelUpUpdate(int currentLevel, int maxXp)
    {
        _levelLabel.Text = "Lvl " + currentLevel;
        _xpBar.MaxValue = maxXp;
    }

    /// <summary>
    /// Update the player health bar when he take or lose health, or its max health change.
    /// </summary>
    /// <param name="currentHealth">Current health of the player.</param>
    /// <param name="currentMaxHealth">Current max health of the player.</param>
    public void UpdateHealthBar(int currentHealth, int currentMaxHealth)
    {
        if (currentHealth != _healthBar.Value) _healthBar.Value = currentHealth;
        if (currentMaxHealth != _healthBar.MaxValue) _healthBar.MaxValue = currentMaxHealth;
    }

    #endregion
}