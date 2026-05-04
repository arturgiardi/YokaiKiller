using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTime : MonoBehaviour {
	public Animator animator;
	public Animator animator2;
	public ParticleSystem ps;
	public ParticleSystem ps2;
	public float timer;
	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(timer);
		animator.SetTrigger("Explode");
		animator2.SetTrigger("Shine");
		ps.Play();
		ps2.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
