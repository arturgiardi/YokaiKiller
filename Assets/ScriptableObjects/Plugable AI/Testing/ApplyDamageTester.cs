using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyDamageTester : MonoBehaviour 
{

	public DamageInfo damagePower;
	public LayerMask damageBlockingLayerMask;
	public Transform damageOrigin;


	void Awake()
	{
		//damagePower.CalculateDamage = damagePower.DefaultDamageFormula;
		//damagePower.CalculateDamage += damagePower.VagrantDamageFormula;
	}

	void OnTriggerEnter(Collider other)
	{
		IDamageable<GameObject,DamageInfo,Stats> damageable = other.GetComponent<IDamageable<GameObject,DamageInfo,Stats>>();

		if(damageable != null)
		{
			if(DamageCanReach(other.transform.position))
			{
				Debug.LogWarning("A porra do ApplyDamageTester por algum motivo ainda é importante e ta tentando causar dano...");
				//damageable.Damage(gameObject, damagePower,);
			}
			else
			{
				DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Red, other.transform.position, 0);
				FindObjectOfType<CameraManager> ().ShakeCamera (0.25f, 0.4f);
			}
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
