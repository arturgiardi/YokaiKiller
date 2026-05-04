using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePopup : MonoBehaviour {
	public enum DamageColor {White, Red, RedCritical, WhiteCritical, Green};

	[SerializeField] GameObject whitePrefab;
	[SerializeField] GameObject whiteCriticalPrefab;
	[SerializeField] GameObject redPrefab;
	[SerializeField] GameObject redCriticalPrefab;
	[SerializeField] GameObject greenPrefab;

	static GameObject _whitePrefab;
	static GameObject _whiteCriticalPrefab;
	static GameObject _redPrefab;
	static GameObject _redCriticalPrefab;
	static GameObject _greenPrefab;
	static Transform parent;

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
		_redPrefab = redPrefab;
		_redCriticalPrefab = redCriticalPrefab;
		_greenPrefab = greenPrefab;
		_whitePrefab = whitePrefab;
		_whiteCriticalPrefab = whiteCriticalPrefab;

	}

	public static void InstantiateDamage(DamageColor color, Vector3 position, float ammount){
		GameObject go = null;
		position.z -= 0.3f;
		switch (color) {
		case DamageColor.White:
			go = Instantiate (_whitePrefab, position, Quaternion.identity, parent) as GameObject;
			break;
		case DamageColor.Red:
			go = Instantiate (_redPrefab, position, Quaternion.identity, parent) as GameObject;
			break;
		case DamageColor.RedCritical:
			go = Instantiate (_redCriticalPrefab, position, Quaternion.identity, parent) as GameObject;
			break;
		case DamageColor.WhiteCritical:
			go = Instantiate (_whiteCriticalPrefab, position, Quaternion.identity, parent) as GameObject;
			break;
		case DamageColor.Green:
			go = Instantiate (_greenPrefab, position, Quaternion.identity, parent) as GameObject;
			break;
		}
		go.transform.GetChild (0).GetComponent<TMP_Text> ().text = ammount.ToString ();
		//go.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = ammount.ToString ();
	}
}
