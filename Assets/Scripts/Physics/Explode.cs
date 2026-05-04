using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {
	[SerializeField]
	Transform explosionCenter;
	[SerializeField]
	float explosionForce;
	[SerializeField]
	float explosionRadius;
	[SerializeField]
	Transform fragmentParent;

	[ContextMenu("Explode")]
	public void ExplodeFragments(){
		foreach (Transform child in fragmentParent) {
			child.GetComponent<Rigidbody> ().useGravity = true;
			child.GetChild(0).GetComponent<MeshCollider>().enabled = true;
			child.GetComponent<Rigidbody> ().AddExplosionForce (explosionForce, explosionCenter.position, explosionRadius);
		}
	}

    public void FastPlaySFX(GameObject sfx)
    {
        SoundEffectInstantiator.PlaySoundFX(sfx, transform.position);
    }
}
