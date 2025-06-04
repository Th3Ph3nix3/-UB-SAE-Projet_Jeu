using Godot;
using System;

[GlobalClass]
public partial class Armor : Passives_Data
{
	public override void EffectUpdate()
	{
	}

	
	
	public override void OnUpgrade()
	{
		holder.armor = upgrades[level].stat;
		GD.Print("Actual armor : ", holder.armor);
	}
}
