using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;// Required when using Event data.
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class LevelUpHud : MonoBehaviour 
{
	[field: SerializeField] public Button HpButton {get; private set;}
	[field: SerializeField] public Button PowerButton {get; private set;}
	[field: SerializeField] public Button LuckButton {get; private set;}
	public TMP_Text powVal, powComp, powRes;
	public TMP_Text hpVal, hpComp, hpRes;
	public TMP_Text lukVal, lukComp, lukRes;

	float curPow;
	float curHp;
	float curLuk;

	private void Start() 
	{
		HpButton.onClick.AddListener(delegate { GetLevelBonus("health"); });
		PowerButton.onClick.AddListener(delegate { GetLevelBonus("power"); });
		LuckButton.onClick.AddListener(delegate { GetLevelBonus("luck"); });
	}

    private void GetLevelBonus(string value)
    {
        PlayerStatsController.instance.GetLevelBonus(value);
    }

    public void SetValuesUp(PlayerStats stats)
	{
		curPow = stats.damage.power;
		curHp = stats.maxHealth;
		curLuk = stats.damage.criticalChance;

		powVal.text = curPow.ToString();
		hpVal.text = curHp.ToString();
		lukVal.text = string.Format("{0:F1}", curLuk/10);
	}

	public void CompareNewStat(string stat)
	{
		switch (stat)
		{
			case "power":
				powComp.text = "<color=green>>";
				hpComp.text = "=";
				lukComp.text = "=";

				powRes.text = "<color=green>"+(curPow+1).ToString();
				hpRes.text = (curHp).ToString();
				lukRes.text = string.Format("{0:F1}", curLuk/10);
			break;

			case "health":
				powComp.text = "=";
				hpComp.text = "<color=green>>";
				lukComp.text = "=";

				powRes.text = (curPow).ToString();
				hpRes.text = "<color=green>"+(curHp+3).ToString();
				lukRes.text = string.Format("{0:F1}", curLuk/10);
			break;

			case "luck":
				powComp.text = "=";
				hpComp.text = "=";
				lukComp.text = "<color=green>>";

				powRes.text = (curPow).ToString();
				hpRes.text = (curHp).ToString();
				lukRes.text = "<color=green>"+string.Format("{0:F1}", (curLuk+1f)/10);
			break;
		}
	}
}
