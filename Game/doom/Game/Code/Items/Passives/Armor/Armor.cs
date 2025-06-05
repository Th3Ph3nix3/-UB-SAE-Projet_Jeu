using Godot;
using System;

[GlobalClass]
public partial class Armor : Passives_Data
{
	public override void EffectUpdate(int level) { return; }

	public override void OnUpgrade(int level)
	{
		if (level == 0) holder.growth += upgrades[level].stat;
        else holder.growth += upgrades[level].stat - upgrades[level - 1].stat;
	}
}
