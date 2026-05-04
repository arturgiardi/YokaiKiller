using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWHitbox : MonoBehaviour
{
	PlayerStatsController playerStats;
	public LayerMask damageBlockingLayerMask;
	public Transform damageOrigin;
	public GameObject damageSFX;
	SimpleAudioPlayer audioP;

	void Awake()
	{
		audioP = GetComponent<SimpleAudioPlayer>();
		playerStats = getPlayerStats (transform);
        damageOrigin = playerStats.origin;			
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
            if (DamageCanReach(other.transform.position))
            {
				DamageInfo finalDamage =  PlayerStatsController.instance.currentSubweapon.GenerateSubweaponDamage();
				damageable.Damage(gameObject, finalDamage, playerStats.stats);
                //damageable.Damage(gameObject, PlayerStatsController.instance.currentSubweapon.subWeaponDamageInfo, playerStats.stats);
                PlayerStatsController.instance.currentSubweapon.ApplySpecialEffect(damageable.GetStateController());
                if (damageable.Living())
                {
                    if (audioP != null && damageSFX != null)
                    {
                        audioP.PlayAudio(damageSFX);
                    }
                }
				Destroy(finalDamage);
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
