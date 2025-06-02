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
        if (PlayerControl.Player.level % NEW_PASSIVE_INTERVAL == 0)
        {
            // Gets all the passives that the player does not have yet
            List<Items> newPassivesItems = new();

            foreach (Passives_Data passives_Data in _passivesList.Where(passive => !PlayerControl.Player.Passives.Any(item => item.Data == passive)).ToList())
            {
                newPassivesItems.Add(new Items(passives_Data));
            }

            if (newPassivesItems.Count > 0)
                {
                    VBoxContainer newItem_Container = new VBoxContainer();
                    newItem_Container.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

                    foreach (Items passive in newPassivesItems)
                    {
                        newItem_Container.AddChild(UpgradeItem_Frame.new_UpgradeItem_Frame(passive, true));
                    }

                    _levelUp_Container.AddChild(newItem_Container);
                }
                else
                    GD.PrintErr("No new passive available for the player.");
        }

        List<Items> upgradableItems = PlayerControl.Player.Passives.Where(item => item.IsUpgradable).ToList();
        if (PlayerControl.Player.Weapon.IsUpgradable) upgradableItems.Add(PlayerControl.Player.Weapon);

        if (upgradableItems.Count > 0)
        {
            VBoxContainer upgradeItem_Container = new VBoxContainer();
            upgradeItem_Container.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

            foreach (Items item in upgradableItems)
            {
                upgradeItem_Container.AddChild(UpgradeItem_Frame.new_UpgradeItem_Frame(item, false));
            }

            _levelUp_Container.AddChild(upgradeItem_Container);
        }
        else
        {
            GD.PrintErr("No upgradable items available for the player.");
        }

        Show();
        GetTree().Paused = true;
    }

    /// <summary>
    /// Close the level up panel by hiding it and freeing all 
    /// </summary>
    public void Close()
    {
        foreach (Node child in _levelUp_Container.GetChildren())
        {
            RemoveChild(child);
            child.QueueFree();
        }

        Hide();
        GetTree().Paused = false;
    }

    #endregion

}

