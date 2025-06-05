using System;
using Godot;

/// <summary>
/// Holds all the base data of an Item.
/// </summary>
[GlobalClass]
public abstract partial class Items_Data : Resource
{
    /// <summary>
    /// Name of the item.
    /// </summary>
    [Export]
    public string name;

    /// <summary>
    /// Texture of the item to be displayed in the UI.
    /// </summary>
    [Export]
    public Texture2D texture;

    /// <summary>
    /// Owner of the item, typically the player character.
    /// </summary>
    public Player holder; // /!\ To change when player is better encapsulated

    /// <summary>
    /// Array of upgrades available for the item.
    /// The first index of the array contains the base stats of the item, and the subsequent indices contain the upgrades.
    /// </summary>
    public abstract Base_Upgrades[] Upgrades { get; }

    /// <summary>
    /// Type of the item, which can be a weapon or passive.
    /// </summary>
    public abstract Items_Type Type { get; }
    
    /// <summary>
    /// Abstract method to be implemented by derived classes to apply the item's effect.
    /// </summary>
    public abstract void EffectUpdate(int level);

	/// <summary>
	/// Called when the item is upgraded.
	/// </summary>
	public abstract void OnUpgrade(int level);
}
