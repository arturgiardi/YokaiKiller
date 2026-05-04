using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

	public void TriggerSelfDestroy()
	{
		Destroy (this.gameObject);
	}
	void OnDisable()
	{
		Destroy (this.gameObject);
	}

}
