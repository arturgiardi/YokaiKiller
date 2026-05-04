using UnityEngine;
using TMPro;

[System.Serializable]
public class ItemStatsField
{
    public TMP_Text statsComparer;
    public TMP_Text statsCurrentValue;
    public TMP_Text statsFinalValue;
}

public class ItemStatsPredict : MonoBehaviour
{
    public delegate void ItemStatsPredictEvent(Item itemData);

    public static ItemStatsPredictEvent OnItemSelect;

    public ItemStatsField lifeStats;
    public ItemStatsField powerStats;
    public ItemStatsField critChanceStats;
    public ItemStatsField critMultiStats;
    public ItemStatsField fireStats;
    public ItemStatsField waterStats;
    public ItemStatsField earthStats;
    public ItemStatsField thunderStats;


    void Awake()
    {
        OnItemSelect += ShowItemStatsPrediction;
    }

    void OnDisable()
    {
        OnItemSelect -= ShowItemStatsPrediction;
        
    }

    void ShowItemStatsPrediction(Item item)
    {   
        if(item != null)
        {
            PlayerStats statsPrediction = PlayerStatsController.instance.GetStatsPrediction(item);
            PlayerStats currentStats = PlayerStatsController.instance.stats;

            CompareStats(lifeStats, currentStats.maxHealth, statsPrediction.maxHealth);
            CompareStats(powerStats, currentStats.damage.power, statsPrediction.damage.power);
            CompareStats(critChanceStats, currentStats.damage.criticalChance/10, statsPrediction.damage.criticalChance/10);
            CompareStats(critMultiStats, currentStats.damage.criticalMultiplier, statsPrediction.damage.criticalMultiplier);

            CompareStats(fireStats, currentStats.damage.elementalInfo.fire*3, statsPrediction.damage.elementalInfo.fire*3);
            CompareStats(waterStats, currentStats.damage.elementalInfo.water, statsPrediction.damage.elementalInfo.water);
            CompareStats(earthStats, currentStats.damage.elementalInfo.earth, statsPrediction.damage.elementalInfo.earth);
            CompareStats(thunderStats, currentStats.damage.elementalInfo.thunder, statsPrediction.damage.elementalInfo.thunder);
        }
        else
        {
            PlayerStats statsPrediction = PlayerStatsController.instance.GetCurrentStats();
            PlayerStats currentStats = PlayerStatsController.instance.stats;

            CompareStats(lifeStats, currentStats.maxHealth, statsPrediction.maxHealth);
            CompareStats(powerStats, currentStats.damage.power, statsPrediction.damage.power);
            CompareStats(critChanceStats, currentStats.damage.criticalChance/10, statsPrediction.damage.criticalChance/10);
            CompareStats(critMultiStats, currentStats.damage.criticalMultiplier, statsPrediction.damage.criticalMultiplier);

            CompareStats(fireStats, currentStats.damage.elementalInfo.fire*3, statsPrediction.damage.elementalInfo.fire*3);
            CompareStats(waterStats, currentStats.damage.elementalInfo.water, statsPrediction.damage.elementalInfo.water);
            CompareStats(earthStats, currentStats.damage.elementalInfo.earth, statsPrediction.damage.elementalInfo.earth);
            CompareStats(thunderStats, currentStats.damage.elementalInfo.thunder, statsPrediction.damage.elementalInfo.thunder);
        }
        
    }

    void CompareStats(ItemStatsField field, float curVal, float newVal)
    {
        if(curVal > newVal)
        {
            field.statsCurrentValue.text = "<color=red>"+curVal;
            field.statsComparer.text = "<color=red><";
            field.statsFinalValue.text = "<color=red>"+newVal;
        }
        else if(curVal < newVal)
        {
            field.statsCurrentValue.text = "<color=green>"+curVal;
            field.statsComparer.text = "<color=green>>";
            field.statsFinalValue.text = "<color=green>"+newVal;
        }
        else
        {
            field.statsCurrentValue.text = "<color=white>"+curVal;
            field.statsComparer.text = "<color=white>=";
            field.statsFinalValue.text = "<color=white>"+newVal;
        }
    }


}