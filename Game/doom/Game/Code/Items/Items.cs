using Godot;
using System;
using System.Xml.Serialization;

/// <summary>
/// Base class for all items in the game.
/// </summary>
public partial class Items : Node
{

	#region attributes

	/// <summary>
	/// Hold all necessary data of the item.
	/// </summary>
	private Items_Data _data;

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
	public Player Holder
	{
		get => _data.holder;
		set
		{
			_data.holder = value;
			_data.OnUpgrade(); // Call the upgrade method to apply changes when the owner is set
		}
	}

	/// <summary>
	/// Get the data of the current loaded item.
	/// </summary>
	public Items_Data Data { get => _data; }

	/// <summary>
	/// Get the current level of the item.
	/// </summary>
	public int Level { get => _data.level; }

	/// <summary>
	/// Get if the item can be upgraded.
	/// </summary>
	public bool IsUpgradable { get => _isUpgradable; }

	/// <summary>
	/// Get the texture of the current loaded item.
	/// </summary>
	public Texture2D Texture { get => _data.texture; }

	/// <summary>
	/// Get the upgrade path of the current loaded item.
	/// </summary>
	public Base_Upgrades[] Upgrades { get => _data.Upgrades; }

	/// <summary>
	/// Get the type of the item, which can be a weapon or passive.
	/// </summary>
	public Items_Type Type { get => _data.Type; }

	#endregion

	#region Events

	/// <summary>
	/// Event raised when the item levels up.
	/// </summary>
	public event Action ItemLeveledUpEvent;

	#endregion

	#region methods

	/// <summary>
	/// Default constructor of Items.
	/// </summary>
	public Items() {}

	/// <summary>
	/// Constructor of items.
	/// </summary>
	/// <param name="items_Data">Data set that is to be used by the item.</param>
	public Items(Items_Data items_Data)
	{
		_data = items_Data;
	}

	/// <summary>
	/// Upgrade the item to the next level if possible.
	/// </summary>
	public void LevelUp()
	{
		if (_isUpgradable)
		{
			_data.level++;
			_data.OnUpgrade(); // Call the upgrade method to apply changes
			ItemLeveledUpEvent?.Invoke();

			if (_data.level >= _data.Upgrades.Length - 1)
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
		if (_data.Upgrades[_data.level].cooldown <= 0)
		{
			return; // No effect to apply if occurence is 0 or less
		}

		_effectTimer += delta;

		if (_effectTimer >= _data.Upgrades[_data.level].cooldown)
		{
			_effectTimer = 0;
			_data.EffectUpdate();
		}
	}

	/// <summary>
	/// Set the weapon's data.
	/// </summary>
	/// <param name="data">Data to set to the weapon.</param>
	public void SetData(Items_Data data)
	{
		if (data == null)
		{
			GD.PrintErr("Items_Data cannot be null.");
			return;
		}

		_data = data;
    }

    #endregion

}
