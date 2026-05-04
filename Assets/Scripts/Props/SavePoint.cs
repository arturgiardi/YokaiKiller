using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour 
{
    [SerializeField]
    private GameObject audioSFX;
    [SerializeField]
    private Material notUsedMaterial;
	[SerializeField]
	private Transform checkPosition;
    [SerializeField]
    private Material usedMaterial;

	[SerializeField]
	ParticleSystem particles;

    //private bool canSave = false;
	public bool showText = true;

    bool saved = false;

    void OnEnable()
    {
        if(particles != null)
			particles.Play();
        saved = false;
    }

    void Interact()
    {
        if(saved)
            return;
        StartCoroutine(_SaveProcess());
        // GameManager.instance.controller.StartSaving();  
        // PlayerStatsController.instance.stats.Heal(999999);
        // PlayerStatsController.instance.SaveState();
		// if(particles != null)
		// 	particles.Stop();
        // if (audioSFX != null)
        //     SoundEffectInstantiator.PlaySoundFX(audioSFX, transform.position);
		// if(showText)
		// 	TextPopup.InstantiateText("CHECK POINT!", GameManager.instance.controller.transform.position + GameManager.instance.controller.GetComponent<CharacterController>().center);
        // saved = true;
    }


    IEnumerator _SaveProcess()
    {
        GameManager.instance.controller.StartSaving();  
        PlayerStatsController.instance.stats.Heal(999999);
        PlayerStatsController.instance.SaveState();
		if(particles != null)
			particles.Stop();
        yield return new WaitForSeconds(2f);
         if (audioSFX != null)
            SoundEffectInstantiator.PlaySoundFX(audioSFX, transform.position);
		if(showText)
			TextPopup.InstantiateText("CHECK POINT!", GameManager.instance.controller.transform.position + GameManager.instance.controller.GetComponent<CharacterController>().center);
        saved = true;
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.controller.EndSaving();  
    }

    /*void OnTriggerEnter(Collider collider)
    {
        if(saved)
            return;
        if (collider.CompareTag("Player"))
        {
            PlayerStatsController.instance.stats.Heal(999999);
            PlayerStatsController.instance.SaveState();
			if(particles != null)
				particles.Stop();
            if (audioSFX != null)
                SoundEffectInstantiator.PlaySoundFX(audioSFX, transform.position);
			if(showText)
				TextPopup.InstantiateText("CHECK POINT!", GameManager.instance.controller.transform.position + GameManager.instance.controller.GetComponent<CharacterController>().center);
            saved = true;
        }
    }*/
}
