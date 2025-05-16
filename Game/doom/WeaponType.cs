using Godot;
using System;

[GlobalClass] // pour rendre la ressource créable depuis l'éditeur
public partial class WeaponType : Resource
{
	
	[Export]
	public string name; // name of the weapon. Only one for now cause we planned to do an "on-enemy-death-reward-system"

	[Export]
	public Texture2D texture; // texture of the weapon

	[Export]
	public float speed; // speed of the weapon

	[Export]
	public float cooldown; // cooldown until the weapon can shoot again

	[Export]
	public float damage; // damage of the weapon
}
