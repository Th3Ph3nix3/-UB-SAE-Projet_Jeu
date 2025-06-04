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
		if (level == 0) holder.MaxHealth += upgrades[level].stat;
        else holder.MaxHealth += upgrades[level].stat - upgrades[level - 1].stat;
	}
}
