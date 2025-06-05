using Godot;
using System;

[GlobalClass]
public partial class MaxHealth : Passives_Data
{
	public override void EffectUpdate(int level)
	{
		return;
	}

	public override void OnUpgrade(int level)
	{
		if (level == 0) holder.MaxHealth += upgrades[level].stat;
        else holder.MaxHealth += upgrades[level].stat - upgrades[level - 1].stat;
	}
}
