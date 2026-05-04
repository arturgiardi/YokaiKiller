using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionCam : MonoBehaviour {
	public float offset;
	public float multiplier;
	public Transform target;
	// Use this for initialization
	void Start () {
		target = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(target.position.x * multiplier + offset, transform.position.y, transform.position.z);
	}
}
