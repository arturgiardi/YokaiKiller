using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecover : MonoBehaviour {
    [SerializeField]
    GameObject pickupClip;

	[SerializeField]
	private float ammount;

	[SerializeField]
	private GameObject toDisable;

	[SerializeField]
	private ParticleSystem gotPS;

	void OnTriggerEnter(Collider other){
		//PlayerStatus.Instance.RecoverHealth (ammount);
		if(other.GetComponent<PlayerStatsController>() != null)
		{
			this.GetComponent<Collider> ().enabled = false;
			toDisable.SetActive (false);
			//gotPS.Play ();
			PlayerFXFeedback.instance.Heal();
			SoundEffectInstantiator.PlaySoundFX(pickupClip, transform.position);
			DamagePopup.InstantiateDamage (DamagePopup.DamageColor.Green, transform.position+Vector3.up/2, ammount);
			PlayerStatsController.instance.stats.Heal(ammount);
		}
	}
}
