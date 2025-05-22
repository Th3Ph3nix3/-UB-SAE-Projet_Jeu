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

			var label = GetNode<Label>("Label");

			TextureNormal = value.texture;
			//label.Text = "Lvl " + ToString(weapon.level + 1);

		}
	}
	#endregion
	#region Methods

	public void _on_gui_input(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("click") == true)
		{
			options.close_options();
		}
	}


	#endregion
}
