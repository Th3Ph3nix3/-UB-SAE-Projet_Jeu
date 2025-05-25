using Godot;
using System;

public partial class OptionSlot : TextureButton
{
	#region Attributes
	private Options _options;
	#endregion
	
	#region Setters / Getters
	public Options options
	{
		get => _options;
		set
		{
			_options = value;
		}
	}
	
	[Export]
	public Weapon _weapon;
	public Weapon weapon
	{
		get => _weapon;
		set
		{
			_weapon = value;
			if (value != null)
			{
				var label = GetNode<Label>("Label");
				var description = GetNode<Label>("Description");
				TextureNormal = value.Texture;
				label.Text = "Lvl " + (weapon.Level + 1).ToString();
				description.Text = value.Upgrades[value.Level - 1].description;
			}
		}
	}
	#endregion
	
	#region Methods
	
	public void _on_gui_input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("click"))
		{
			if (weapon != null)
			{
				weapon.UpgradeItem(); // upgrade the weapon
			}
			
			if (_options != null)
			{
				_options.close_options();
			}
			else
			{
				GD.PrintErr("Options reference is null in OptionSlot");
			}
		}
	}
	#endregion
}
