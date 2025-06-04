using Godot;
using System;

[GlobalClass]
public partial class MaxHealth : Passives_Data
{
	public override void EffectUpdate()
	{
		return;
	}

	public override void OnUpgrade()
	{
		holder.health = upgrades[level].stat;
		GD.Print("Health recovered : ", holder.health);
	}
}
