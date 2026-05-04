using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour {
    public AudioClip[] clipList;
    public float pitchVariation;


	//public void PlaySoundFX(GameObject prefab, Vector3 position){
		//GameObject audioFX = Instantiate(prefab, position, Quaternion.identity) as GameObject;
		//StartCoroutine(_WaitTillEnd(audioFX));
	//}


	//IEnumerator _WaitTillEnd(GameObject player){
		//AudioSource source = player.GetComponent<AudioSource>();
		//while(source.isPlaying){
			//yield return null;
		//}
		//Destroy(source);
	//}

	//public void FastPlaySFX(GameObject sfx){
		//PlaySoundFX (sfx, transform.position);
	//}


    void Awake()
    {
        AudioSource audioComp = GetComponent<AudioSource>();
        audioComp.pitch = Random.Range(audioComp.pitch-pitchVariation, audioComp.pitch+pitchVariation);
        audioComp.PlayOneShot(clipList[Random.Range(0, clipList.Length)]);
        StartCoroutine(_WaitForSoundEnd(audioComp));
    }

    IEnumerator _WaitForSoundEnd(AudioSource audioComp)
    {
        yield return null;
        while (audioComp.isPlaying)
        {
            yield return null;
        }
        Destroy(gameObject);
    }

}
