using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour, IDamageable<GameObject, DamageInfo, Stats>
{

	StateController enemyController;
	EnemyStats enemyStats;
	PlayerStats playerStats;

	void Start()
	{
		enemyController = getStats (transform);
		if(enemyController != null)
			enemyStats = enemyController.stats;
		if(enemyStats == null)
		{
			playerStats = getPlayerStats (transform);
		}
		else
		{
			enemyController.ResetEvent += ResetEnemyHurtbox;
		}
	}

	void ResetEnemyHurtbox(EnemyStats newStats)
	{
		enemyStats = newStats;
	}

	StateController getStats(Transform target)
	{
		if(target == null)
			return null;
		StateController stateController = target.GetComponent<StateController>();
		if (stateController == null)
			return getStats (target.parent);
		else
			return stateController;
	}

	PlayerStats getPlayerStats(Transform target)
	{
		if (target.GetComponent<PlayerStatsController> ()  == null)
			return getPlayerStats (target.parent);
		else
			return target.GetComponent<PlayerStatsController>().stats;
	}

	public void Damage(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		if(enemyStats != null)
		{
			if(enemyStats.OnDamage != null)
			{
				enemyStats.OnDamage(instigator, damage, attacker);
			}
		}
		else
		{
			if(playerStats.OnDamage != null)
			{
				playerStats.OnDamage(instigator, damage, attacker);
			}
		}
		
	}

	public bool Living()
	{
		return true;
	}

    public StateController GetStateController()
    {
        return enemyController;
    }



}
