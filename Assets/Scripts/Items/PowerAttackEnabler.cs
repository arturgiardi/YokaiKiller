using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAttackEnabler : MonoBehaviour {

	[SerializeField]
	int uniqueID = 0;

	[SerializeField]
	private float ammount;

	[SerializeField]
	private GameObject toDisable;

	[SerializeField]
	private ParticleSystem gotPS;

	[SerializeField]
	[TextArea(1, 10)] string itemText;
	[SerializeField]
	string itemName;

	public GameObject soundFX;



	void Awake()
	{
		if (uniqueID != 0) 
		{
			//if (PlayerStatus.Instance.WasCollected (uniqueID))
			// 	Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerStatsController> ()) {
		 	PlayerStats.instance.skillFlags.havePowerAttack = true;
		 	this.GetComponent<Collider> ().enabled = false;
		 	toDisable.SetActive (false);
			gotPS.Play ();
			GetComponent<SimpleAudioPlayer>().PlayAudio(soundFX);
			GameManager.instance.itemValidator.obtainedIds.Add(1);
		// 	if (uniqueID != 0) {
		// 		PlayerStatus.Instance.CollectItem (uniqueID);
		// 	}
		 	SpecialItemDialog.instance.SpanText (itemName, itemText);
		}
	}
}
