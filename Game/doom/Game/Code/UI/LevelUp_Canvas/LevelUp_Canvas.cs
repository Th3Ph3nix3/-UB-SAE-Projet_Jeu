using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using System.IO;


public partial class LevelUp_Canvas : CanvasLayer
{

    #region attributes

    /// <summary>
    /// Player of the last player how called Open().
    /// </summary>
    private Player _player;

    /// <summary>
	/// To add particles in the background when a level up happen.
	/// </summary>
    private GpuParticles2D _particles;

    /// <summary>
    /// To add a panel in the background when a level up happen.
    /// </summary>
    private NinePatchRect _panel;

    /// <summary>
    /// Container for the level up options, including new items and upgrades.
    /// </summary>
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
        Hide(); // Hide the level up panel initially

        _particles = GetNode<GpuParticles2D>("Particles");
        _panel = GetNode<NinePatchRect>("Panel");
        _levelUp_Container = GetNode<HBoxContainer>("LevelUp_Container");

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
    public void Open(Player player)
    {
        _player = player;
        bool toShow = false; // Will be set true if something has to be displayed.

        // Get all upgradable items for the player, including weapon and passives
        List<Items> upgradableItems = player.Passives.Where(item => item.IsUpgradable).ToList();
        if (player.Weapon.IsUpgradable) upgradableItems.Add(player.Weapon);

        if (upgradableItems.Count > 0)
        {
            toShow = true;

            VBoxContainer upgradeContainer = new VBoxContainer();
            upgradeContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

            foreach (Items item in upgradableItems)
            {
                // Create the necessary item frame and connect the canvas to it.
                UpgradeItem_Frame upgradeFrame = UpgradeItem_Frame.new_UpgradeItem_Frame(item, false);
                upgradeFrame.ItemClickedEvent += (sender, e) => ItemClicked(e.Item, e.IsNew);
                upgradeContainer.AddChild(upgradeFrame);
            }

            _levelUp_Container.AddChild(upgradeContainer);
        }
        else
        {
            GD.PrintErr("No upgradable items available for the player.");
        }

        if (player.Level % Global.NEW_PASSIVE_INTERVAL == 0)
        {
            List<Items> newPassivesItems = new();

            // Gets all the passives that the player does not have yet
            foreach (Passives_Data passives_Data in _passivesList.Where(passive => !player.Passives.Any(item => item.Data == passive)).ToList())
            {
                newPassivesItems.Add(new Items(passives_Data));
            }

            if (newPassivesItems.Count > 0)
            {
                toShow = true;

                VBoxContainer newItemContainer = new VBoxContainer();
                newItemContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;

                foreach (Items item in newPassivesItems)
                {
                    // Create the necessary item frame and connect the canvas to it.
                    UpgradeItem_Frame upgradeFrame = UpgradeItem_Frame.new_UpgradeItem_Frame(item, true);
                    upgradeFrame.ItemClickedEvent += (sender, e) => ItemClicked(e.Item, e.IsNew);
                    newItemContainer.AddChild(upgradeFrame);
                }

                _levelUp_Container.AddChild(newItemContainer);
            }
            else
                GD.PrintErr("No new passive available for the player.");
        }

        if (toShow)
        {
            Show();
            GetTree().Paused = true;
        }
    }

    /// <summary>
    /// Called when a UpgradeFrame as been clicked, upgrade or add a item to the player.
    /// Then close the level up canvas.
    /// </summary>
    /// <param name="item">Item on which the player clicked.</param>
    /// <param name="isNew">True if the item is a new item, false otherwise.</param>
    public void ItemClicked(Items item, bool isNew)
    {
        if (isNew) _player.AddItem(item);
        else item.LevelUp();
        Close();
    }

    /// <summary>
    /// Close the level up canvas by hiding it and freeing all children.
    /// </summary>
    private void Close()
    {
        foreach (VBoxContainer UpgradeContainer in _levelUp_Container.GetChildren())
        {
            foreach (Node UpgradeFrame in UpgradeContainer.GetChildren())
            {
                UpgradeFrame.QueueFree();
            }

            UpgradeContainer.QueueFree();
        }

        Hide();
        GetTree().Paused = false;
    }

    #endregion

}

