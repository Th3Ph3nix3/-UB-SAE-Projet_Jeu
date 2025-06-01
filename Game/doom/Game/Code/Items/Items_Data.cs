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
    /// Upgrades of the item.
    /// </summary>
    public abstract Upgrades[] Upgrades { get; }

    /// <summary>
    /// Owner of the item, typically the player character.
    /// </summary>
    public PlayerControl holder; // /!\ To change when player is better encapsulated

    /// <summary>
	/// Current level of the item. Start at 0.
	/// </summary>
	public int level = 0;

    /// <summary>
	/// Abstract method to be implemented by derived classes to apply the item's effect.
	/// </summary>
	public abstract void EffectUpdate();

	/// <summary>
	/// Called when the item is upgraded.
	/// </summary>
	public abstract void OnUpgrade();
}
