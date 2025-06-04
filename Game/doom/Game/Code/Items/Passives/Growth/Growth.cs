using Godot;
using System;

[GlobalClass]
public partial class Growth : Passives_Data
{
	
	public override void EffectUpdate()
	{
		holder.growth += (int)upgrades[level].stat;
		GD.Print("Actual xp : ", holder.growth);
	}

	/// <summary>
	/// This passives doesn't have an effect on upgrade.
	/// </summary>
	public override void OnUpgrade()
	{
		return;
	}
}
