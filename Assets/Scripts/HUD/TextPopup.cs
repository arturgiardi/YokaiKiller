using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPopup : MonoBehaviour {

	[SerializeField]
	GameObject prefab;

	static Transform parent;
	static GameObject _prefab;
	static MonoBehaviour mono;

	void Start(){
		//Check if instance already exists
		if (mono == null)

			//if not, set instance to this
			mono = this;

		//If instance already exists and it's not this:
		else if (mono != this) {
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);
			return;
		}
		mono = this;
		parent = this.transform;
		_prefab = prefab;
	}

	public static void InstantiateText(string text, Vector3 position){
		GameObject go = null;
		position.z -= 0.3f;
		go = Instantiate (_prefab, position, Quaternion.identity, parent) as GameObject;
		go.transform.GetChild (0).GetComponent<Text> ().text = text;
	}
}
