using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedBreakable : MonoBehaviour, IDamageable<GameObject, DamageInfo, Stats>
{

	[SerializeField] Transform explosionCenter;
	[SerializeField] float explosionForce;
	[SerializeField] float explosionRadius;
	[SerializeField] Transform fragmentParent;
	[SerializeField] Collider collider;
	[SerializeField] ParticleSystem ps;
	[SerializeField] SimpleAudioPlayer aPlayer;
	[SerializeField] GameObject hitSFX;
	[SerializeField] GameObject breakSFX;


	bool broken = false;

	public void Damage(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		Debug.Log("????");
		if(broken)
			return;
		ps.Play();
		Debug.Log("Atingido");
		Debug.Log(damage.charged);
		//GameManager.instance.cameraController.ShakeCamera(0.1f, 0.02f);
		
		if(damage.charged)
		{
			aPlayer.PlayAudio(breakSFX);
			ExplodeFragments();
			FindObjectOfType<CameraManager> ().ShakeCamera(0.35f, 20);
		}
		else
		{
			aPlayer.PlayAudio(hitSFX);
			//FindObjectOfType<CameraManager> ().ShakeCamera(0.1f, 0.25f);
		}
			

	}

	void ExplodeFragments(){
		broken = true;
		collider.enabled = false;
		foreach (Transform child in fragmentParent) {
			child.GetComponent<Rigidbody> ().useGravity = true;
			child.GetChild(0).GetComponent<MeshCollider>().enabled = true;
			child.GetComponent<Rigidbody> ().AddExplosionForce (explosionForce, explosionCenter.position, explosionRadius);
		}
	}

    public StateController GetStateController()
    {
        return null;
    }


    public bool Living()
	{
		return false;
	}

}
