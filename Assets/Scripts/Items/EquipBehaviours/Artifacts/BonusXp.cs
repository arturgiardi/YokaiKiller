using UnityEngine;

[CreateAssetMenu (menuName = "Item/Behaviour/Bonus Xp")]
public class BonusXp : OnEquipBehaviour
{
    public float bonusVal;
    public override void EquipItem(PlayerStats stats, Item item)
    {
        PlayerStats.instance.xpMultiplier += bonusVal;
    }

    public override void RemoveItem(PlayerStats stats, Item item)
    {
    }

}