using Godot;
using System;

[GlobalClass]
public partial class PassiveItem : Item
{
    [Export]
    private Stats[] upgrades = Array.Empty<Stats>();
    public PlayerControl player_reference;

    [Export]
    public Texture2D Texture {get; set; }

    public bool is_upgradable()
    {
        return (level <= upgrades.Length);
    }

    public void upgrade_item()
    {
        if (!is_upgradable() || player_reference == null)
        {
            return;
        }
        var current_upgrade = upgrades[level - 1];

        // Upgrade stats and Level Up !
        player_reference.max_health += current_upgrade.max_health; // increased max health
        player_reference.recovery += current_upgrade.recovery; // recover an HP amout per second
        player_reference.armor += current_upgrade.armor; // damage reduction
        player_reference.movement_speed += current_upgrade.movement_speed; // increased movement speed
        player_reference.might += current_upgrade.might; // deals more damages
        player_reference.area += current_upgrade.area; // increased area to detect enemies
        player_reference.magnet += current_upgrade.magnet; // increased area for catching drops
        player_reference.growth += (int)current_upgrade.growth; // increased XP when catching drops

        level++;
    }
}
