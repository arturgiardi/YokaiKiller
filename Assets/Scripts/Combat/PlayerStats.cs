using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SkillFlags
{
	public bool havePowerAttack = false;
	public bool hasAirDash = false;
	public bool hasDoubleJump = false;

	public SkillFlags (bool power, bool airDash, bool doubleJump)
	{
		havePowerAttack = power;
		hasAirDash = airDash;
		hasDoubleJump = doubleJump;
	}

}
[CreateAssetMenu (menuName = "Player Stats")]
public class PlayerStats : Stats 
{

	//Event Triggered on earning XP
	public delegate void OnEarnXPEvent(float ammount, float percentageToNext);
	public OnEarnXPEvent OnEarnXP;

	public delegate void OnEarnLevelEvent(int level);
	public OnEarnLevelEvent OnEarnLevel;

	public delegate void OnStatsChangeEvent();
	public static OnStatsChangeEvent OnStatsChange;

	public delegate void OnSpiritChangeEvent(float currentAmmoun);
	public static OnSpiritChangeEvent OnSpiritChange;

	public float spirit;

	public SkillFlags skillFlags;
	public DamageInfo damage;
	public DefenseInfo defense;

	float currentXP = 0;
	public float xpMultiplier = 1;

	int currentLevel = 1;
	public static PlayerStats instance;

	public override void Death(GameObject instigator, DamageInfo damage, Stats attacker)
	{

	}

	public override void Damage(GameObject instigator, DamageInfo damage, Stats attacker)
	{

		DamageInfo finalDamageInfo = Stats.CalculateDamage(damage, defense, attacker, this);
		float finalDamage = finalDamageInfo.ammount;

		#if UNITY_EDITOR
			Debug.Log("<color=green><b>Player</b></color> took <color=brown><b>" + finalDamage + "</b></color> damage from <color=brown><b>" + instigator.name +"</b></color>");
		#endif

		if(currentHealth <= 0 && OnDeath != null)
		{
			OnDeath(instigator,damage,attacker);
		}

		else
		{
			#if UNITY_EDITOR
				Debug.Log("<color=green><b>Player</b></color> has <color=green><b>" + currentHealth + "</b></color> health left!");
			#endif
		}

		if(finalDamageInfo.ammount > 0)
		{
			if(finalDamageInfo.critical)
			{
				FindObjectOfType<CameraManager> ().ShakeCamera (finalDamageInfo.contactTime*(finalDamageInfo.criticalMultiplier/2)*3, finalDamageInfo.intensity*(finalDamageInfo.criticalMultiplier/2)*3);
				DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Red, PlayerStatsController.instance.origin.position, finalDamageInfo.ammount);
			}
			else
			{
				FindObjectOfType<CameraManager> ().ShakeCamera (finalDamageInfo.contactTime*3, finalDamageInfo.intensity*2);
				Debug.Log(PlayerStatsController.instance);
				DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Red, PlayerStatsController.instance.origin.position, finalDamageInfo.ammount);
			}
		}

		currentHealth -= finalDamage;
		currentHealth = (float)(System.Math.Round((double)currentHealth, 1));	

		if(OnChangeHP != null)
		{
			OnChangeHP(maxHealth, currentHealth);
		}
        if(currentHealth <= 0 && OnDeath != null)
			OnDeath(instigator,damage,attacker);
	}

	public static PlayerStats Setup(PlayerStats initialStats)
	{
		if(instance == null)
		{
			Debug.Log("Instanciando Stats");

			DamageInfo damageInstance = UnityEngine.Object.Instantiate(initialStats.damage) as DamageInfo;
			DefenseInfo defenseInstance = UnityEngine.Object.Instantiate(initialStats.defense) as DefenseInfo;
			PlayerStats statsInstance = UnityEngine.Object.Instantiate(initialStats) as PlayerStats;

			statsInstance.damage = damageInstance;
			statsInstance.defense = defenseInstance;

			statsInstance.currentHealth = statsInstance.maxHealth;

			instance = statsInstance;

 			instance.OnDamage += instance.Damage;

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
		}
		return instance;
	}

	public void AddXP(float ammount)
	{
		ammount *= xpMultiplier;
		currentXP = currentXP + ammount;
		#if UNITY_EDITOR
					Debug.Log("<color=green><b>You got " + ammount + " xp!</b></color>");
		#endif
		if (currentLevel < GetLevel (currentXP)) {
			//LevelFeedback ();
			//MaxHealth += 10;
			//attackPower += 2;
			//HealthGuiManager.instance.IncreaseMaxHealth (10);
			//HealthGuiManager.instance.SetLevel (currentLevel);
			currentLevel++;	
			#if UNITY_EDITOR
				Debug.Log("<color=green><b>You reached level " + currentLevel + "</b></color>");
			#endif
			if(OnEarnLevel != null)
			{
				OnEarnLevel(currentLevel);
			}
		}
		if(OnEarnXP != null)
			OnEarnXP(ammount, GetPercentageXpToNextLevel());
		experience = currentXP;
		level = currentLevel;
	}

	public int GetLevel(float xpAmmount){
		float xpToNext = XpToNextLevel(currentLevel);
		if (xpAmmount > xpToNext)
			return currentLevel + 1;
		else
			return currentLevel;
	}

	public float XpToNextLevel(int level)
	{
		return (50 * level) + ((50 * level)*((50 * level * 0.05f)));
	}

	public void UpdateXP()
	{
		currentXP = experience;
		currentLevel = level;
		if(OnEarnXP != null)
			OnEarnXP(0, GetPercentageXpToNextLevel());
	}

	public void Heal(float ammount)
	{
		currentHealth += ammount;
		if(currentHealth > maxHealth)
			currentHealth = maxHealth;

		currentHealth = (float)(System.Math.Round((double)currentHealth, 1));		

		if(OnChangeHP != null)
		{
			OnChangeHP(maxHealth, currentHealth);
		}
	}

	public float GetPercentageXpToNextLevel()
	{
		float realCurrentXp = currentXP - XpToNextLevel(currentLevel - 1);
		float realNext = XpToNextLevel(currentLevel) - XpToNextLevel(currentLevel - 1);
		return realCurrentXp/realNext;
	}

	public void AddEquipment(Item equipment)
	{
		if(equipment != null)
		{
			if(equipment.equipBehaviour != null)
			{
				equipment.equipBehaviour.EquipItem(this, equipment);
				if(OnStatsChange != null)
					OnStatsChange();
			}
		}
	}

	public void RemoveEquipment(Item equipment)
	{
		if(equipment != null)
		{
			if(equipment.equipBehaviour != null)
			{
				equipment.equipBehaviour.RemoveItem(this, equipment);
				if(OnStatsChange != null)
					OnStatsChange();
			}
		}
	}

	public void AddSpirit(float value)
	{
		spirit += value;
	}

	public static PlayerStats ResetStats(PlayerStats baseStats)
	{
		DamageInfo damageInstance = UnityEngine.Object.Instantiate(baseStats.damage) as DamageInfo;
		DefenseInfo defenseInstance = UnityEngine.Object.Instantiate(baseStats.defense) as DefenseInfo;
		//PlayerStats statsInstance = UnityEngine.Object.Instantiate(baseStats) as PlayerStats;

		instance.damage = damageInstance;
		instance.defense = defenseInstance;

		// statsInstance.OnChangeHP = toCopy.OnChangeHP;
		// statsInstance.OnDamage = toCopy.OnDamage;
		// statsInstance.OnDeath = toCopy.OnDeath;
		// statsInstance.OnEarnLevel = toCopy.OnEarnLevel;
		// statsInstance.OnEarnXP = toCopy.OnEarnXP;

		instance.maxHealth = baseStats.maxHealth;

		//statsInstance.currentHealth = instance.currentHealth;
		//statsInstance.currentLevel = instance.currentLevel;
		//statsInstance.currentXP = instance.currentXP;

		//instance = statsInstance;

		if(instance.OnDamage == null)
			instance.OnDamage += instance.Damage;

		instance.DamagePass.Clear();
		//if(instance.DamagePass == null)
		instance.DamagePass = new List<Func<DamageInfo, DamageInfo>>();
		instance.DamagePass.Add(DamageInfo.DefaultDamageFormula);

		instance.CriticalPass.Clear();
		//if(statsInstance.CriticalPass == null)
		instance.CriticalPass = new List<Func<DamageInfo, DamageInfo>>();
		instance.CriticalPass.Add(DamageInfo.DefaultCriticalFormula);

		instance.DefensePass.Clear();
		//if(statsInstance.DefensePass == null)
		instance.DefensePass = new List<Func<DamageInfo, DefenseInfo, DamageInfo>>();
		instance.DefensePass.Add(DamageInfo.DefauldDefenseFormula);

		instance.ElementalPass.Clear();
		//if(statsInstance.ElementalPass == null)
		instance.ElementalPass = new List<Func<DamageInfo, DefenseInfo, DamageInfo>>();
		instance.ElementalPass.Add(DamageInfo.DefaultElementalFormula);

		if(OnStatsChange != null)
			OnStatsChange();

		return instance;
	}

	public void Unset()
	{
		OnSpiritChange=null;
		OnChangeHP=null;
		OnDamage=null;
		OnDeath=null;
		OnEarnLevel=null;
		OnEarnXP=null;
		OnStatsChange = null;
		instance = null;
	}
	
	

}
