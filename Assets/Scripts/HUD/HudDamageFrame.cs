using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudDamageFrame : MonoBehaviour 
{

	Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		// while(PlayerStatsController.instance.stats == null)
		// {
		// 	yield return null;
		// }
		var stats = PlayerStatsController.instance.stats;
		stats.OnDamage += OnDamageTaken;
	}

	void OnDamageTaken(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		animator.SetTrigger("Damage");
	}
	
}
