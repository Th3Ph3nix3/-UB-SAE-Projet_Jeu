using System;
using System.Collections.Generic;
using System.Linq;
using Godot;


public partial class LevelUp_Panel : CanvasLayer
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

    #endregion

    #region methods

    /// <summary>
    /// Open the level up panel and display the upgrade options for the player.
    /// If the player is at a level that allows for a new passive item, it will also display that option.
    /// </summary>
    public void Open()
    {
        if (Global.PlayerManager.Player.level % NEW_PASSIVE_INTERVAL == 0)
        {
            // Gets all the passives that the player does not have yet
            List<Passives_Data> newPassives = Global.Database.PassivesList.Where(passive => !Global.PlayerManager.Player.Passives.Any(item => item.Data == passive)).ToList();

            if (newPassives.Count > 0)
            {
                VBoxContainer newItem_Container = new VBoxContainer();
                // newItem_Container.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                newItem_Container.ProcessMode = ProcessModeEnum.Always;

                foreach (Passives_Data passive in newPassives)
                {
                    newItem_Container.AddChild(NewItem_Frame.new_NewItem_Frame(passive));
                }

                _levelUp_Container.AddChild(newItem_Container);
            }
            else
                GD.PrintErr("No new passive available for the player.");
        }

        List<Items> upgradableItems = Global.PlayerManager.Player.Passives.Where(item => item.IsUpgradable).ToList();
        if (Global.PlayerManager.Player.Weapon.IsUpgradable) upgradableItems.Add(Global.PlayerManager.Player.Weapon);

        if (upgradableItems.Count > 0)
        {
            VBoxContainer upgradeItem_Container = new VBoxContainer();
            // upgradeItem_Container.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            upgradeItem_Container.ProcessMode = ProcessModeEnum.Always;

            foreach (Items item in upgradableItems)
            {
                upgradeItem_Container.AddChild(UpgradeItem_Frame.new_UpgradeItem_Frame(item));
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
            if (child is UpgradeItem_Container || child is NewItem_Container)
            {
                RemoveChild(child);
                child.QueueFree();
            }
        }

        Hide();
        GetTree().Paused = false;
    }

    #endregion

}

