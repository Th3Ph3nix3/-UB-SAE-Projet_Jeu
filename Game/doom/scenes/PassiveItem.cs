using Godot;
using System;

[GlobalClass]
public partial class PassiveItem : Item
{
	[Export]
	private Stats[] _upgrades = Array.Empty<Stats>();

	public Stats[] Upgrades
	{
		get => _upgrades;
	}
	public bool is_upgradable()
	{
		return (Level <= _upgrades.Length);
	}

	public void upgrade_item(PlayerControl player)
	{
		if (!is_upgradable() || player == null)
		{
			GD.PrintErr("Cannot upgrade item or player reference is null.");
			return;
		}
		Stats current_upgrade = _upgrades[Level - 1];

		// Upgrade stats and Level Up !
		player.max_health += current_upgrade.max_health; // increased max health
		player.recovery += current_upgrade.recovery; // recover an HP amout per second
		player.armor += current_upgrade.armor; // damage reduction
		player.movement_speed += current_upgrade.movement_speed; // increased movement speed
		player.might += current_upgrade.might; // deals more damages
		player.area += current_upgrade.area; // increased area to detect enemies
		player.magnet += current_upgrade.magnet; // increased area for catching drops
		player.growth += (int)current_upgrade.growth; // increased XP when catching drops

		Level++;
	}
}
