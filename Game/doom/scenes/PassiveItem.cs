using Godot;
using System;

[GlobalClass]
public partial class PassiveItem : Item
{
    [Export]
    private Stats[] upgrades = Array.Empty<Stats>();
    public PlayerControl player_reference;

    public bool is_upgradable()
    {
        return (level <= upgrades.Length);
    }

    public void uprgade_item()
    {
        if (!is_upgradable() || player_reference == null)
        {
            return;
        }
        var current_upgrade = upgrades[level - 1];

        // Upgrade stats and Level Up !
        player_reference.max_health += current_upgrade.max_health;
        player_reference.recovery += current_upgrade.recovery;
        player_reference.armor += current_upgrade.armor;
        player_reference.movement_speed += current_upgrade.movement_speed;
        player_reference.might += current_upgrade.might;
        player_reference.area += current_upgrade.area;
        player_reference.magnet += current_upgrade.magnet;
        player_reference.growth += (int)current_upgrade.growth;

        level++;
    }
}
