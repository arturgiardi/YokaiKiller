using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu (menuName = "Stats/Damage")]
public class DamageInfo : ScriptableObject
{
	//public delegate float DamageFormula(DefenseInfo defense);
	//public DamageFormula CalculateDamage;
	public int ammount;
	public int power;
	public float positiveVariation = 0;
	public float negativeVariation = 0;
	public float intensity;
	public float contactTime;
	public float criticalChance;
	public float criticalMultiplier;
	public float chargedMultiplier;
	public ElementalInfo elementalInfo;
	public float criticalBuild = 0;
	public bool critical;
	public bool charged = false;

	public int healthPlus;

	public DamageInfo (int _ammount = 0, float _positiveVariation = 0, float _negativeVariation = 0,
		float _intensity = 2f, float _contactTime = 0.1f, Vector3 _origin = default(Vector3),
		GameObject _instigator = default(GameObject), GameObject _hitEffect = default(GameObject),
		float _criticalMultiplier = 1.5f, bool _critical = false)
	{
		ammount = _ammount;
		positiveVariation = _positiveVariation;
		negativeVariation = _negativeVariation;
		intensity = _intensity;
		contactTime = _contactTime;
		critical = _critical;
		criticalMultiplier = _criticalMultiplier;
	}

	public static DamageInfo DefaultDamageFormula(DamageInfo input)
	{
		DamageInfo returnInfo = Object.Instantiate(input) as DamageInfo;
		float finalDamage = returnInfo.power;
		float damageVariation = Random.Range(returnInfo.negativeVariation, returnInfo.positiveVariation);
		finalDamage += damageVariation;

		if(input.charged)
			finalDamage *= input.chargedMultiplier;

		returnInfo.ammount = Mathf.RoundToInt(finalDamage);

		return returnInfo;
	}

	public static DamageInfo DefaultCriticalFormula(DamageInfo input)
	{
		DamageInfo returnInfo = Object.Instantiate(input) as DamageInfo;
		float critRandom = Random.Range((int)0,(int)1000);
		returnInfo.criticalBuild += critRandom;
		if(returnInfo.criticalBuild > 1000-returnInfo.criticalChance)
		{
			returnInfo.critical = true;
		}
		return returnInfo;
	}

	public static DamageInfo DefauldDefenseFormula(DamageInfo input, DefenseInfo defense)
	{
		DamageInfo returnInfo = Object.Instantiate(input) as DamageInfo;
		float armorReduction = defense.armor;
		returnInfo.ammount -= Mathf.RoundToInt(armorReduction);
		if(returnInfo.critical)
		{
			returnInfo.ammount *= Mathf.RoundToInt(returnInfo.criticalMultiplier);
		}
		returnInfo.ammount = returnInfo.ammount < 1 ? 1 : returnInfo.ammount;
		returnInfo.ammount = Mathf.RoundToInt(returnInfo.ammount);
		return returnInfo;
	}

	public static DamageInfo DefaultElementalFormula(DamageInfo input, DefenseInfo defense)
	{
		DamageInfo returnInfo = Object.Instantiate(input) as DamageInfo;

		///*
		var fireDamage = (returnInfo.power*(returnInfo.elementalInfo.fire/10));
		fireDamage -= fireDamage*(defense.elementalInfo.fire/10);

		var watterDamage = (returnInfo.power*(returnInfo.elementalInfo.water/10));
		watterDamage -= watterDamage*(defense.elementalInfo.water/10);

		var earthDamage = (returnInfo.power*(returnInfo.elementalInfo.earth/10));
		earthDamage -= earthDamage*(defense.elementalInfo.earth/10);

		var thunderDamage = (returnInfo.power*(returnInfo.elementalInfo.thunder/10));
		thunderDamage -= thunderDamage*(defense.elementalInfo.thunder/10);
		//*/

		/*
		var fireDamage = returnInfo.elementalInfo.fire;
		fireDamage -= defense.elementalInfo.fire;
		if(fireDamage < 0)
			fireDamage = 0;

		var watterDamage = returnInfo.elementalInfo.water;
		watterDamage -= defense.elementalInfo.water;
		if(watterDamage < 0)
			watterDamage = 0;
		
		var earthDamage = returnInfo.elementalInfo.earth;
		earthDamage -= defense.elementalInfo.earth;
		if(earthDamage < 0)
			earthDamage = 0;

		var thunderDamage = returnInfo.elementalInfo.thunder;
		thunderDamage -= defense.elementalInfo.thunder;
		if(thunderDamage < 0)
			thunderDamage = 0;
		*/

		returnInfo.ammount = Mathf.RoundToInt(returnInfo.ammount + fireDamage + watterDamage + earthDamage + thunderDamage);

		return returnInfo;
	}
	
	

	public float VagrantDamageFormula(DefenseInfo defense)
	{
		int comboCount = 10;
		float finalDamage = power;
		finalDamage += Random.Range(negativeVariation, positiveVariation);
		float critRandom = Random.Range((int)0,(int)100);
		//Debug.Log(critRandom);
		critical = critRandom < (criticalChance/(comboCount/10));

		if(critical)
			finalDamage *= criticalMultiplier+(comboCount/20);

		finalDamage -= (defense.armor + Random.Range(defense.negativeVariation,defense.postitiveVariation));

		if(finalDamage < 1)
			finalDamage = 1;

		finalDamage = (float)(System.Math.Round((double)finalDamage, 1));	
		//Combo ++
		return finalDamage;
	}
}

