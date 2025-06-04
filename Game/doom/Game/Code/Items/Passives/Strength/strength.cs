using Godot;
using System;

[GlobalClass]
public partial class Strength : Weapons_Data
{
	public override void EffectUpdate()
	{
		projectile = null;
    }

	/// <summary>
	/// This passives doesn't have an effect on upgrade.
	/// </summary>
	public override void OnUpgrade()
	{
		return;
	}
}
