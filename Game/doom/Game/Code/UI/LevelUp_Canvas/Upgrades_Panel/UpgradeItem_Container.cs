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
    /// <summary>
    /// When the UpgradeItem_Container is initiated, it creates a new UpgradeItem_Frame for each item in the list.
    /// This is used to display the available upgrades for the player.
    /// The items are passed as a list, and a boolean indicates whether the item is new or not.
    /// It automatically expands to fill the available horizontal space in the UI.
    /// </summary>
    /// <param name="items">Items list to add in the container.</param>
    /// <param name="newItem">Set if the items in the list are new or not.</param>
    public UpgradeItem_Container(List<Items> items, bool newItem)
    {
        this.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        foreach (Items item in items) AddChild(UpgradeItem_Frame.new_UpgradeItem_Frame(item, newItem));
    }

    /// <summary>
    /// Called when the node is removed from the scene tree.
    /// It queues all child nodes for deletion to clean up the UI.
    /// </summary>
    public void Close()
    {
        foreach (Node child in GetChildren())
        {
            child.QueueFree();
        }
    }
}
