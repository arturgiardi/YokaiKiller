using UnityEngine;

[CreateAssetMenu(menuName = "Item/Behaviour/Soul's Magnifier")]
public class SoulsMagnifier : OnEquipBehaviour
{
    public override void EquipItem(PlayerStats stats, Item item)
    {
        PlayerStatsController.instance.specialEffects.healthInspector = true;
        GameManager.instance.bossHp.ammount.gameObject.SetActive(true);

        if (GameManager.instance.bossHp.on)
        {
            GameManager.instance.bossHp.ShowBossHP();
        }

        Debug.Log("Equipando");
    }

    public override void RemoveItem(PlayerStats stats, Item item)
    {
        PlayerStatsController.instance.specialEffects.healthInspector = false;
        GameManager.instance.bossHp.ammount.gameObject.SetActive(false);

        if (GameManager.instance.bossHp.on)
        {
            GameManager.instance.bossHp.ShowBossHP();
        }
        Debug.Log("Removendo");
    }

}