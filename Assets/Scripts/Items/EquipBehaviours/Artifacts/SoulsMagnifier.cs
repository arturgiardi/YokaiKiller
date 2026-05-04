using UnityEngine;

[CreateAssetMenu (menuName = "Item/Behaviour/Soul's Magnifier")]
public class SoulsMagnifier : OnEquipBehaviour
{
    public override void EquipItem(PlayerStats stats, Item item)
    {
        PlayerStatsController.instance.specialEffects.healthInspector = true;
        GameManager.instance.bossHp.animator.SetBool("MagOn", true);
        if(GameManager.instance.bossHp.on)
        {
            GameManager.instance.bossHp.ShowBossHP();
        }
            
        Debug.Log("Equipando");
    }

    public override void RemoveItem(PlayerStats stats, Item item)
    {
        PlayerStatsController.instance.specialEffects.healthInspector = false;
        GameManager.instance.bossHp.animator.SetBool("MagOn", false);
        if(GameManager.instance.bossHp.on)
        {
            GameManager.instance.bossHp.ShowBossHP();
        }
        Debug.Log("Removendo");
    }

}