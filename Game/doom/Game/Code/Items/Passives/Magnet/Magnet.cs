using Godot;
using System;

[GlobalClass]
public partial class Magnet : Passives_Data
{
	public override void EffectUpdate()
	{
		return;
	}

	public override void OnUpgrade()
	{
		if (level == 0) holder.magnet += upgrades[level].stat;
        else holder.magnet += upgrades[level].stat - upgrades[level - 1].stat;
	}
}
