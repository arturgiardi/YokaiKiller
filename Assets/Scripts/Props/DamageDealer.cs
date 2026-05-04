using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

	public DamageInfo damageInfo;
	EnemyStats statsInfo;
	public LayerMask damageBlockingLayerMask;
	public Transform damageOrigin;
	public GameObject damageSFX;
	SimpleAudioPlayer audioP;

	void Awake()
	{
		audioP = GetComponent<SimpleAudioPlayer>();
		statsInfo = ScriptableObject.CreateInstance<EnemyStats>();
		statsInfo.damage = Instantiate(damageInfo) as DamageInfo;
		statsInfo.DamagePass = new List<System.Func<DamageInfo, DamageInfo>>();
		statsInfo.DamagePass.Add(DamageInfo.DefaultDamageFormula);
		statsInfo.CriticalPass = new List<System.Func<DamageInfo, DamageInfo>>();
		statsInfo.CriticalPass.Add(DamageInfo.DefaultCriticalFormula);
		statsInfo.ElementalPass = new List<System.Func<DamageInfo, DefenseInfo, DamageInfo>>();
		statsInfo.ElementalPass.Add(DamageInfo.DefaultElementalFormula);
	}

	void OnTriggerEnter(Collider other)
	{
		IDamageable<GameObject,DamageInfo,Stats> damageable = other.GetComponent<IDamageable<GameObject,DamageInfo,Stats>>();
		if(damageable != null)
		{

			if(DamageCanReach(other.transform.position))
			{
				damageable.Damage(gameObject, statsInfo.damage, statsInfo);
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
				DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Red, other.transform.position, 0);				
			}
		}
	}

	public void Damage (IDamageable<GameObject,DamageInfo,Stats> damageable, Transform other)
	{
		if(DamageCanReach(other.position))
		{
			damageable.Damage(gameObject, statsInfo.damage, statsInfo);
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
			DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Red, other.transform.position, 0);				
		}
	}
	bool DamageCanReach(Vector3 otherPosition)
	{
		Ray ray = new Ray(damageOrigin.position, otherPosition - damageOrigin.position);

		if(Physics.Raycast(ray, Vector3.Distance(damageOrigin.position, otherPosition), damageBlockingLayerMask))
			return false;
		else
			return true;

	}
	

}
