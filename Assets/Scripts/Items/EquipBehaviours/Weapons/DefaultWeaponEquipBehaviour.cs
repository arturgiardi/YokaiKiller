using UnityEngine;
[CreateAssetMenu (menuName = "Item/Behaviour/Weapon Default")]
public class DefaultWeaponEquipBehaviour : OnEquipBehaviour
{
    public override void EquipItem(PlayerStats stats, Item item)
    {
        stats.maxHealth += item.iStats.damage.healthPlus;

        stats.damage.power += item.iStats.damage.power;
        stats.damage.criticalChance += item.iStats.damage.criticalChance;
        stats.damage.criticalMultiplier += item.iStats.damage.criticalMultiplier;

        stats.damage.elementalInfo.fire += item.iStats.damage.elementalInfo.fire;
        stats.damage.elementalInfo.water += item.iStats.damage.elementalInfo.water;
        stats.damage.elementalInfo.earth += item.iStats.damage.elementalInfo.earth;
        stats.damage.elementalInfo.thunder += item.iStats.damage.elementalInfo.thunder;
    }

    public override void RemoveItem(PlayerStats stats, Item item)
    {
        return;
    }

}