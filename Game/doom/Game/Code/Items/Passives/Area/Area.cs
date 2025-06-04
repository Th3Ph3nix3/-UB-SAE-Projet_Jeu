using Godot;
using System;

[GlobalClass]
public partial class Area : Passives_Data
{

	public override void EffectUpdate()
	{
		holder.area += upgrades[level].stat;
	}

	public override void OnUpgrade()
	{
		return;
	}
}
