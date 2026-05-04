using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliderSwitch : MonoBehaviour {
	[SerializeField]
	private Transform reference;
	[SerializeField]
	private BoxCollider colliderObject;

	//public Transform refa;
	//public Transform refb;

	private CharacterController body;
	// Use this for initialization
	void Start () {
		//reference = GameManager.instance.controller.transform;
		reference = FindObjectOfType<NewController>().transform;
		body = reference.GetComponent<CharacterController> ();

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 footPosition = (reference.position + body.center);
		footPosition.y -= (body.height / 2);
		float topPosition = colliderObject.transform.position.y + ((colliderObject.size.y/2)*colliderObject.transform.lossyScale.y) + colliderObject.center.y;
		//print (colliderObject.size);
		if (!colliderObject.enabled) {
			if (footPosition.y >= topPosition + 0.1f) {
				colliderObject.enabled = true;
			} else {
				colliderObject.enabled = false;
			}
		}
		else {
			if (footPosition.y >= topPosition-0.1f) {
				
				colliderObject.enabled = true;
			} else {
				colliderObject.enabled = false;
			}
		}

		//if (refa != null && refb != null) {
		//	refa.position = footPosition;
		//	refb.position = new Vector3(transform.position.x, topPosition, transform.position.z);
		//}
	}
}
