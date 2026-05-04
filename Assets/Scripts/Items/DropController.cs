using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour {
	[System.Serializable]
	public class DropableItem{
		[Range(0, 1000)]
		public int probability;
		public GameObject prefab;
	}

	[SerializeField]
	DropableItem[] dropTable;
	[SerializeField]
	Transform dropPoint;


	public void DropItems(){

		foreach (DropableItem dropable in dropTable) {

			float roll = Random.Range (0, 1000);

			if (roll <= dropable.probability)
				ThrowDrop (dropable.prefab);
				

		}
	}

	void ThrowDrop(GameObject item){
		

		if (item.GetComponent<Rigidbody> ()) {
			GameObject dropedItem = Instantiate (item, dropPoint.position, Quaternion.identity) as GameObject;
			Vector3 throwForce = new Vector3 (Random.Range (-3f, 3f), Random.Range (4, 6), 0);
			//Vector3 throwForce = new Vector3 (Random.Range (0, 0), Random.Range (2, 3), 0);
			dropedItem.GetComponent<Rigidbody> ().AddForce (throwForce, ForceMode.Impulse);
		} 
		else {
			Debug.Log ("Dropable does not contain a Rigidbody!: " + item.name);
		}

	}


}
