using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class Stats : ScriptableObject 
{
	//Base Stats
	[HideInInspector] public bool alive = true;
	public int level = 1;
	public float experience = 0;
    public float maxHealth;
    [HideInInspector] public float currentHealth;

	//Event Triggered on death
	public delegate void OnDeathEvent(GameObject instigator, DamageInfo damage, Stats attacker);
	public OnDeathEvent OnDeath;

    // Event triggered on damage
    public delegate void OnDamageTaken(GameObject instigator, DamageInfo damage, Stats attacker);
    public OnDamageTaken OnDamage;

	public delegate void OnChangeHPEvent(float maxHP, float currentHP);
	public OnChangeHPEvent OnChangeHP;

	public List<Func<DamageInfo, DamageInfo>> DamagePass;
	public List<Func<DamageInfo, DamageInfo>> CriticalPass;
	public List<Func<DamageInfo, DefenseInfo, DamageInfo>> DefensePass; 
	public List<Func<DamageInfo, DefenseInfo, DamageInfo>> ElementalPass;

	public void Setup()
	{
		currentHealth = maxHealth;
		OnDamage += Damage;
		OnDeath += Death;
	}

	public abstract void Death(GameObject instigator, DamageInfo damage, Stats attacker);
	public abstract void Damage(GameObject instigator, DamageInfo damage, Stats attacker);

	public static DamageInfo CalculateDamage(DamageInfo damage, DefenseInfo defense, Stats attacker, Stats defender)
	{
		int damPasses = attacker.DamagePass.Count;
		int defPasses = defender.DefensePass.Count;
		int critpasses = attacker.DamagePass.Count;
		int elPasses = defender.ElementalPass.Count;
		//Debug.LogWarning(damPasses);
		//Debug.LogWarning(defPasses);
		//Debug.LogWarning(critpasses);
		//Debug.LogWarning(elPasses);
		DamageInfo returnDamageInfo = UnityEngine.Object.Instantiate(damage) as DamageInfo;
		DefenseInfo returnDefenseInfo = UnityEngine.Object.Instantiate(defense) as DefenseInfo;
		foreach(Func<DamageInfo, DamageInfo> damagePass in attacker.DamagePass)
		{
			returnDamageInfo = damagePass(returnDamageInfo);
		}
		foreach(Func<DamageInfo, DamageInfo> criticalPass in attacker.CriticalPass)
		{
			returnDamageInfo = criticalPass(returnDamageInfo);
		}
		foreach(Func<DamageInfo, DefenseInfo, DamageInfo> defensePass in defender.DefensePass)
		{
			returnDamageInfo = defensePass(returnDamageInfo, returnDefenseInfo);
		}
		foreach(Func<DamageInfo, DefenseInfo, DamageInfo> elementalPass in defender.ElementalPass)
		{
			returnDamageInfo = elementalPass(returnDamageInfo, returnDefenseInfo);
		}


		return returnDamageInfo;

	}


}
