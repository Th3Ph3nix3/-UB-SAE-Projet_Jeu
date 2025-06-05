using System;
using System.Collections.Generic;
using Godot;
using System.IO;

public partial class WeaponChoice_Canvas : CanvasLayer
{

    #region attributes 

    /// <summary>
    /// Weapons available to the player.
    /// </summary>
    private List<Weapons_Data> _weaponsList = new();

    private HBoxContainer _weapon_Container;

    #endregion

    #region events

    /// <summary>
    /// Event to notify a weapon as been choosed.
    /// </summary>
    public event EventHandler<WeaponChoosedEventArgs> WeaponChoosedEvent;

    /// <summary>
    /// Class to pass to WeaponChoosed event data.
    /// Contain the weapon that have been choosed.
    /// </summary>
    public class WeaponChoosedEventArgs : EventArgs
    {
        public Items Weapon { get; }
        public WeaponChoosedEventArgs(Items weapon)
        { Weapon = weapon; }
    }

    #endregion

    #region methods

    /// <summary>
    /// Scan the weapons .tres in the corresponding weapons folder in Resource.
    /// </summary>
    public override void _Ready()
    {
        _weapon_Container = GetNode<HBoxContainer>("Weapons_Container");

        foreach (string file in Directory.GetFiles("Game/Resource/Weapons/"))
        {
            if (Path.GetExtension(file) == ".tres")
            {
                _weaponsList.Add(GD.Load<Weapons_Data>(file));
            }
        }

        foreach (Weapons_Data weapon in _weaponsList)
        {
            UpgradeItem_Frame weaponFrame = UpgradeItem_Frame.new_UpgradeItem_Frame(new Items(weapon));
            weaponFrame.ItemClickedEvent += (sender, e) => WeaponChoosedEventTransit(e.Item);
            _weapon_Container.AddChild(weaponFrame);
        }

        GD.PrintErr(_weapon_Container.GetChildCount());

        Show();
        GetTree().Paused = true;
    }

    public void WeaponChoosedEventTransit(Items itemSend)
    {
        WeaponChoosedEvent?.Invoke(this, new WeaponChoosedEventArgs(itemSend));
        Hide();
        GetTree().Paused = false;
    }

    #endregion
}