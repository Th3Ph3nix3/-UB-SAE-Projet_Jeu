using System;
using System.Collections.Generic;
using Godot;

public partial class UI : CanvasLayer
{
    /// <summary>
    /// Reference to the player.
    /// </summary>
    [Export]
    private Player _player;

    /// <summary>
    /// Canvas that holds all UI element informing the player.
    /// </summary>
    private PlayerInfo_Canvas _playerInfo_Canvas;

    /// <summary>
    /// Canvas that displays the level up options when the player levels up.
    /// </summary>
    private LevelUp_Canvas _levelUp_Canvas;

    /// <summary>
    /// Canvas that displays the weapons the player can choose at the start of the game.
    /// </summary>
    private WeaponChoice_Canvas _weaponChoice_Canvas;

    #region methods

    public override void _Ready()
    {
        _playerInfo_Canvas = GetNode<PlayerInfo_Canvas>("PlayerInfo_Canvas");
        _levelUp_Canvas = GetNode<LevelUp_Canvas>("LevelUp_Canvas");
        _weaponChoice_Canvas = GetNode<WeaponChoice_Canvas>("WeaponChoice_Canvas");

        _player.LevelUpEvent += LevelUpEvent;
        _player.ItemAddedEvent += ItemAddedEvent;
        _player.XpGainedEvent += XpGainedEvent;
        _player.HealthChangedEvent += HealthChangedEvent;

        _weaponChoice_Canvas.WeaponChoosedEvent += _player.WeaponChoosedEvent;

        _playerInfo_Canvas.LevelUpUpdate(0, Global.BASE_XP_TO_GET);
        _playerInfo_Canvas.UpdateHealthBar(Global.BASE_MAX_PLAYER_HEALTH, Global.BASE_MAX_PLAYER_HEALTH);
    }

    /// <summary>
    /// Function openning the level up canvas and updating the xp label when the player level up.
    /// </summary>
    public void LevelUpEvent(object sender, Player.LevelUpEventArgs e)
    {
        _levelUp_Canvas.Open((Player)sender);
        _playerInfo_Canvas.LevelUpUpdate(e.CurrentLvl, e.CurrentNextLevelXpNeeded);
    }

    /// <summary>
    /// Function adding a new item to the item display. Also link it to the new item.
    /// </summary>
    public void ItemAddedEvent(object sender, Player.ItemAddedEventArgs e)
    {
        _playerInfo_Canvas.AddItemDisplay(e.AddedItem);
    }

    /// <summary>
    /// Update the xp bar when the player gain xp.
    /// </summary>
    public void XpGainedEvent(object sender, Player.XpGainedEventArgs e)
    {
        _playerInfo_Canvas.UpdateXpBar(e.CurrentXp);
    }

    public void HealthChangedEvent(object sender, Player.HealthChangedEventArgs e)
    {
        _playerInfo_Canvas.UpdateHealthBar(e.CurrentHealth, e.CurrentMaxHealth);
    }

    

    #endregion

}