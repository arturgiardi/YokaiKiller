using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakGround : MonoBehaviour {
	public GameObject deactive;
	public GameObject active;
	public GameObject[] fragments;
	public float delay;

	bool broke;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void BreakGroun () {
		if (!broke) 
		{
			deactive.SetActive (false);
			active.SetActive (true);
			StartCoroutine (_Fragmentation (delay));
			broke = true;
		}

	}


	IEnumerator _Fragmentation(float delay)
	{
		yield return new WaitForSeconds (1.2f);
		foreach (GameObject fragment in fragments) {
			fragment.GetComponentInChildren<ParticleSystem> ().Play ();
			fragment.GetComponent<Rigidbody> ().isKinematic = false;
			fragment.GetComponent<Rigidbody> ().useGravity = true;
			yield return new WaitForSeconds (delay);
		}
	}
}
