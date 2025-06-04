using Godot;
using System;

[GlobalClass]
public partial class Area : Passives_Data
{

	public override void EffectUpdate() { return; }

	public override void OnUpgrade()
	{
		if (level == 0) holder.growth += upgrades[level].stat;
        else holder.growth += upgrades[level].stat - upgrades[level - 1].stat;
	}
}
