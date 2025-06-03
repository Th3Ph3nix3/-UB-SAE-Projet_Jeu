using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using System.IO;


public partial class LevelUp_Canvas : CanvasLayer
{

    #region attributes

    /// <summary>
	/// Interval of level between the possibily to add a new passive item.
	/// </summary>
	private const int NEW_PASSIVE_INTERVAL = 3;

    /// <summary>
	/// To add particles in the background when a level up happen.
	/// </summary>
	[Export]
    private GpuParticles2D _particles;

    /// <summary>
    /// To add a panel in the background when a level up happen.
    /// </summary>
    [Export]
    private NinePatchRect _panel;

    /// <summary>
    /// Container for the level up options, including new items and upgrades.
    /// </summary>
    [Export]
    private HBoxContainer _levelUp_Container;

    /// <summary>
    /// Array of all passive items in the game.
    /// </summary>
    private List<Passives_Data> _passivesList = new();

    /// <summary>
    /// Array of all weapons in the game.
    /// </summary>
    private List<Weapons_Data> _weaponsList = new();

    #endregion

    #region methods

    /// <summary>
    /// When LevelUp _Canvas is initiated, get all passives and weapons created in the corresponding resource folder.
    /// </summary>
    public override void _Ready()
    {
        this.Hide(); // Hide the level up panel initially

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

    /// <summary>
    /// Open the level up panel and display the upgrade options for the player.
    /// If the player is at a level that allows for a new passive item, it will also display that option.
    /// </summary>
    public void Open()
    {
        // Get all upgradable items for the player, including weapon and passives
        List<Items> upgradableItems = Player.Ref.Passives.Where(item => item.IsUpgradable).ToList();
        if (Player.Ref.Weapon.IsUpgradable) upgradableItems.Add(Player.Ref.Weapon);

        if (upgradableItems.Count > 0)
        {
            _levelUp_Container.AddChild(new UpgradeItem_Container(upgradableItems, false));
        }
        else
        {
            GD.PrintErr("No upgradable items available for the player.");
        }

        if (Player.Ref.level % NEW_PASSIVE_INTERVAL == 0)
        {
            List<Items> newPassivesItems = new();

            // Gets all the passives that the player does not have yet
            foreach (Passives_Data passives_Data in _passivesList.Where(passive => !Player.Ref.Passives.Any(item => item.Data == passive)).ToList())
            {
                newPassivesItems.Add(new Items(passives_Data));
            }

            if (newPassivesItems.Count > 0)
            {
                _levelUp_Container.AddChild(new UpgradeItem_Container(newPassivesItems, true));
            }
            else
                GD.PrintErr("No new passive available for the player.");
        }

        Show();
        GetTree().Paused = true;
    }

    /// <summary>
    /// Close the level up panel by hiding it and freeing all 
    /// </summary>
    public void Close()
    {
        foreach (UpgradeItem_Container child in _levelUp_Container.GetChildren())
        {
            child.Close();
            child.QueueFree();
        }

        Hide();
        GetTree().Paused = false;
    }

    #endregion

}

