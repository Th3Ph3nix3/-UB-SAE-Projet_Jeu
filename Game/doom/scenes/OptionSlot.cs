using Godot;
using System;

public partial class OptionSlot : TextureButton
{
	#region setters

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
	#region methods

	// public void _on_gui_input(InputEvent inputEvent){
	// 	if (inputEvent.IsActionPressed("click") == true && weapon != null)
	// 	{
	// 		GD.Print(weapon.title);
	// 		GetParent().;
	// 	}
	// }


	#endregion
}
