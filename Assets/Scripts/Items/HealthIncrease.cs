using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : MonoBehaviour 
{

	[SerializeField]
	int uniqueID = -1;

	[SerializeField]
	private int ammount;

	[SerializeField]
	private GameObject toDisable;

	[SerializeField]
	private ParticleSystem gotPS;

	public string itemName;
	[TextArea()] public string itemText;
	public GameObject soundFX;


	void Awake()
	{
		if (uniqueID != 0) 
		{
			// if (PlayerStatus.Instance.WasCollected (uniqueID))
			// 	Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerStatsController> ()) {
		 	PlayerStatsController.instance.baseStats.maxHealth += ammount;
			PlayerStats.instance.currentHealth += ammount;
			PlayerStatsController.instance.GetCurrentStats();
		 	this.GetComponent<Collider> ().enabled = false;
		 	toDisable.SetActive (false);
			gotPS.Play ();
			GetComponent<SimpleAudioPlayer>().PlayAudio(soundFX);
		// 	if (uniqueID != 0) {
		// 		PlayerStatus.Instance.CollectItem (uniqueID);
		// 	}
			GameManager.instance.itemValidator.obtainedIds.Add(uniqueID);
			string text = string.Format(itemText,ammount);
		 	SpecialItemDialog.instance.SpanText (itemName, text);
		}
	}
}
