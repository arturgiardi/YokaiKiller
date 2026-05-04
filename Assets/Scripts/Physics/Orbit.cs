using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {

	public Transform center;
	public float rotationSpeed = 80.0f;
	public Rigidbody body;
	float xrot;
	float zrot;

	void Start () {
	}

	void Update () { 
		xrot = Mathf.PingPong (xrot, 1);
		zrot = Mathf.PingPong (zrot, 1);
		transform.Rotate (new Vector3 (xrot, 1f, zrot) * rotationSpeed * Time.deltaTime);
	}
}