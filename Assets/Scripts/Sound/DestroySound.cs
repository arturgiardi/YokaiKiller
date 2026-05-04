using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour {
	AudioSource audiS;
	// Use this for initialization
	void Start () {
		audiS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(audiS.isPlaying)
			return;
		Destroy(this.gameObject);
	}
}
