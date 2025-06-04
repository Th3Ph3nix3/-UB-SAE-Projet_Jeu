using Godot;
using System;

[GlobalClass]
public partial class Growth : Passives_Data
{
	
	public override void EffectUpdate() { return; }

	/// <summary>
	/// This passives doesn't have an effect on upgrade.
	/// </summary>
	public override void OnUpgrade()
	{
		if (level == 0) holder.growth += upgrades[level].stat;
        else holder.growth += upgrades[level].stat - upgrades[level - 1].stat;
	}
}
