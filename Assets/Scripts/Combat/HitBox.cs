using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

	StateController enemyStats;
	PlayerStatsController playerStats;
	public LayerMask damageBlockingLayerMask;
	public Transform damageOrigin;
	public GameObject damageSFX;
	SimpleAudioPlayer audioP;

	void Awake()
	{
		audioP = GetComponent<SimpleAudioPlayer>();
		enemyStats = getStats (transform);
		if(enemyStats == null)
		{
			playerStats = getPlayerStats (transform);
			//playerStats.stats.damage.CalculateDamage = playerStats.stats.damage.DefaultDamageFormula;
			damageOrigin = playerStats.origin;
		}
		else
		{
			//enemyStats.stats.damage.CalculateDamage = enemyStats.stats.damage.DefaultDamageFormula;
			damageOrigin = enemyStats.origin;
			enemyStats.ResetEvent += ResetHitbox;	
		}
			
	}

	void ResetHitbox(EnemyStats newStats)
	{
		Awake();
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

	PlayerStatsController getPlayerStats(Transform target)
	{
		if(target == null)
			return null;
		PlayerStatsController statsController = target.GetComponent<PlayerStatsController>();
		if (statsController  == null)
			return getPlayerStats (target.parent);
		else
			return statsController;
	}

	void OnTriggerEnter(Collider other)
	{
		IDamageable<GameObject,DamageInfo,Stats> damageable = other.GetComponent<IDamageable<GameObject,DamageInfo,Stats>>();
		if(damageable != null)
		{

			if(DamageCanReach(other.transform.position))
			{
				if(enemyStats != null) //-> Enemy 
				{
					damageable.Damage(gameObject, enemyStats.stats.damage, enemyStats.stats);
				}
				else
				{
					damageable.Damage(gameObject, playerStats.stats.damage, playerStats.stats);
				} 
				if(damageable.Living())
				{
					if(audioP != null && damageSFX != null)
					{
						audioP.PlayAudio(damageSFX);
					}
				}
			}
			else
			{
				print("can't reach" + other.name);
				if(enemyStats != null)
				{
					
					//DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Red, other.transform.position, 0);
					//FindObjectOfType<CameraManager> ().ShakeCamera (0.25f, 0.4f);
				}
				else
				{
					//DamagePopup.InstantiateDamage (DamagePopup.DamageColor.White, other.transform.position, 0);
					//FindObjectOfType<CameraManager> ().ShakeCamera (0.25f, 0.4f);
				}
				
			}
		}
	}

	bool DamageCanReach(Vector3 otherPosition)
	{
		Ray ray = new Ray(damageOrigin.position, otherPosition - damageOrigin.position);
		RaycastHit hit = new RaycastHit();
		if(Physics.Raycast(ray, out hit, Vector3.Distance(damageOrigin.position, otherPosition), damageBlockingLayerMask))
		{
			Debug.Log(hit.transform.gameObject);
			return false;
		}
		else
			return true;

	}
	

}
