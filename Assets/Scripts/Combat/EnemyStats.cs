using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Enemy/Enemy Stats")]
public class EnemyStats : Stats 
{
	
	public string enemyName;
	public float activationRange;
	public float activationHeight;
	public float interactionRange;
	public float wallDetectionRange;
	public float leapRange;
	public float leapDepth;
	public bool showLifeBar;
	public bool showName;
	public bool boss;
	public DamageInfo damage;
	public DefenseInfo defense;


	public override void Death(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		alive = false;
	}

	public override void Damage(GameObject instigator, DamageInfo damage, Stats attacker)
	{

	}

	public EnemyStats GetInstance(Stats stats)
	{
		DamageInfo damageInstance = UnityEngine.Object.Instantiate(damage) as DamageInfo;
		DefenseInfo defenseInstance = UnityEngine.Object.Instantiate(defense) as DefenseInfo;
		EnemyStats statsInstance = UnityEngine.Object.Instantiate(stats) as EnemyStats;

		statsInstance.damage = damageInstance;
		statsInstance.defense = defenseInstance;

		// if(damageInstance.CalculateDamage == null)
		// 	damageInstance.CalculateDamage = damageInstance.DefaultDamageFormula;
		if(statsInstance.DamagePass == null)
			statsInstance.DamagePass = new List<Func<DamageInfo, DamageInfo>>();
		statsInstance.DamagePass.Add(DamageInfo.DefaultDamageFormula);

		if(statsInstance.CriticalPass == null)
			statsInstance.CriticalPass = new List<Func<DamageInfo, DamageInfo>>();
		statsInstance.CriticalPass.Add(DamageInfo.DefaultCriticalFormula);

		if(statsInstance.DefensePass == null)
			statsInstance.DefensePass = new List<Func<DamageInfo, DefenseInfo, DamageInfo>>();
		statsInstance.DefensePass.Add(DamageInfo.DefauldDefenseFormula);

		if(statsInstance.ElementalPass == null)
			statsInstance.ElementalPass = new List<Func<DamageInfo, DefenseInfo, DamageInfo>>();
		statsInstance.ElementalPass.Add(DamageInfo.DefaultElementalFormula);


		return statsInstance;
	}


}
