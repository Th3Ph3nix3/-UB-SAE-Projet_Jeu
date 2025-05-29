using Godot;
using System;
using System.Xml.Serialization;

/// <summary>
/// Base class for all items in the game.
/// </summary>
public abstract partial class Items : Resource
{

	#region attributes

	/// <summary>
	/// Owner of the item, typically the player character.
	/// </summary>
	protected PlayerControl _owner; // /!\ To change when player is better encapsulated

	/// <summary>
	/// Name of the item.
	/// </summary>
	[Export]
	private string _name;

	/// <summary>
	/// Texture of the item to be displayed in the UI.
	/// </summary>
	[Export]
	private Texture2D _texture;

	/// <summary>
	/// Current level of the item. Start at 1.
	/// </summary>
	private int _level = 0;

	/// <summary>
	/// Flag indicating if the item can be upgraded.
	/// </summary>
	private bool _isUpgradable = true;

    /// <summary>
    /// Internal timer to manage the effect application timing.
    /// </summary>
    private double _effectTimer = 0;

    #endregion

    #region properties

    /// <summary>
    /// Set the owner of the item.
    /// </summary>
    public PlayerControl Owner
	{
		set
		{
			_owner = value;
			OnUpgrade(); // Call the upgrade method to apply changes when the owner is set
		}
    }

    /// <summary>
    /// Get the texture of the item.
    /// </summary>
    public Texture2D Texture
	{
		get => _texture;
	}

	/// <summary>
	/// Get the current level of the item.
	/// </summary>
	public int Level
	{
		get => _level;
	}

	/// <summary>
	/// Get if the item can be upgraded.
	/// </summary>
	public bool IsUpgradable
	{
		get => _isUpgradable;
	}

	/// <summary>
	/// Obligatory getter of the upgrades of the item.
	/// Necessary for the IsUpgradable method to work which is common to all items.
	/// </summary>
	protected abstract Upgrades[] Upgrades { get; }

	#endregion

	#region methods

	/// <summary>
	/// Upgrade the item to the next level if possible.
	/// </summary>
	public void LevelUp()
	{
		if (_isUpgradable)
		{
			_level++;
            OnUpgrade(); // Call the upgrade method to apply changes

            if (_level >= Upgrades.Length)
			{
				_isUpgradable = false;
			}
		}
	}

    /// <summary>
    /// Updates the effect timer of the passive item. Returns if the occurence is 0 or less, meaning no effect will be applied.
    /// </summary>
    /// <param name="delta">Time elapsed since the last frame.</param>
    public void UpdateEffectTimer(double delta)
    {
        if (Upgrades[_level].cooldown <= 0)
        {
            return; // No effect to apply if occurence is 0 or less
        }

        _effectTimer += delta;

        if (_effectTimer >= Upgrades[_level].cooldown)
        {
            _effectTimer = 0;
            EffectUpdate();
        }
    }

    /// <summary>
    /// Abstract method to be implemented by derived classes to apply the item's effect.
    /// </summary>
    protected abstract void EffectUpdate();

	/// <summary>
	/// Called when the item is upgraded.
	/// </summary>
	protected virtual void OnUpgrade() { return; }

    #endregion

}
