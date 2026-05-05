using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

	public Rigidbody rb;
	public LayerMask detectionLayerMask;
	public float testForce;

	bool moving = true;

	void Start()
	{
		if(testForce > 0)
			ShootProjectile(testForce);
	}

	void Update()
	{
		if(moving)
		{
			transform.forward = rb.linearVelocity;
			Collider[] detected = new Collider[0];
			detected = Physics.OverlapBox(transform.position, new Vector3(0.12f, 0.12f, 0.12f), Quaternion.identity, detectionLayerMask);
			if(detected.Length > 0)
			{
				IDamageable<GameObject,DamageInfo,Stats> damageable = detected[0].GetComponent<IDamageable<GameObject,DamageInfo,Stats>>();
				if(damageable == null)
				{
					moving = false;
					rb.Sleep();
					rb.constraints = RigidbodyConstraints.FreezeAll;
					rb.isKinematic = true;
					this.GetComponent<Collider>().enabled = false;
				}
				else
				{
					GetComponent<DamageDealer>().Damage(damageable, detected[0].transform);
					Destroy(gameObject);
				}
			}
		}
	}
	
	public void ShootProjectile(float force)
	{
		force = force == 0 ? testForce : force;
		Vector3 projectileForce = transform.forward * force;
		rb.AddForce(projectileForce, ForceMode.Impulse);
	}

	void OnTriggerEnter(Collider other)
	{
		IDamageable<GameObject,DamageInfo,Stats> damageable = other.GetComponent<IDamageable<GameObject,DamageInfo,Stats>>();
		if(damageable == null)
		{
			moving = false;
			rb.Sleep();
			rb.constraints = RigidbodyConstraints.FreezeAll;
			rb.isKinematic = true;
			this.GetComponent<Collider>().enabled = false;
		}
		else
		{
			Destroy(gameObject);
		}
		
	}


}
