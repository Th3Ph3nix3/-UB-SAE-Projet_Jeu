using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Options class to manage the upgrade options for the player.
/// It allows the player to upgrade their weapons and passives by showing a panel with available upgrades.
/// </summary>
public partial class UpgradeItem_Container : VBoxContainer
{

    public UpgradeItem_Container(List<Items> items, bool newItem)
    {
        foreach (Items item in items) AddChild(UpgradeItem_Frame.new_UpgradeItem_Frame(item, newItem));
    }
}
