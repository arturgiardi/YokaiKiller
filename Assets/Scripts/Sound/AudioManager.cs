using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static AudioManager instance;


	public AudioSource music;
    public AudioSource ambientSound;
    [SerializeField]
	AudioSource generalEffects;
	

	IEnumerator volCoroutine;


	void Awake(){
		if(instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	public void ChangeVolume(float velocity, float value){
		if(value > music.volume){
			if(volCoroutine != null)
				StopCoroutine(volCoroutine);
			volCoroutine = _RaiseVolume(velocity,value);
			StartCoroutine(volCoroutine);
		}
		else{
			if(volCoroutine != null)
				StopCoroutine(volCoroutine);
			volCoroutine = _LowerVolume(velocity,value);
			StartCoroutine(volCoroutine);
		}
	}

	IEnumerator _LowerVolume(float velocity, float decreaseTo){
		while(music.volume > decreaseTo + 0.01f){
			music.volume -= Time.deltaTime*velocity;
			yield return null;
		}
		music.volume = decreaseTo;
	}

	IEnumerator _RaiseVolume(float velocity, float increaseTo){
		while(music.volume < increaseTo - 0.01f){
			music.volume += Time.deltaTime*velocity;
			yield return null;
		}
		music.volume = increaseTo;
	}

	public void PlayEffect(AudioClip clip){
		generalEffects.PlayOneShot(clip);
	}

	public void RestartMusic(){
		music.Stop();
		music.Play();
	}

    public void SetMusic(AudioClip newClip, bool willLoop)
    {
        music.Stop();
        music.clip = newClip;
        music.loop = willLoop;
        music.Play();
    }

	public void ChangeAmbientVolume(float velocity, float value){
		if(value > ambientSound.volume){
			if(volCoroutine != null)
				StopCoroutine(volCoroutine);
			volCoroutine = _RaiseAmbientVolume(velocity,value);
			StartCoroutine(volCoroutine);
		}
		else{
			if(volCoroutine != null)
				StopCoroutine(volCoroutine);
			volCoroutine = _LowerAmbientVolume(velocity,value);
			StartCoroutine(volCoroutine);
		}
	}

	IEnumerator _LowerAmbientVolume(float velocity, float decreaseTo){
		while(ambientSound.volume > decreaseTo + 0.01f){
			ambientSound.volume -= Time.deltaTime*velocity;
			yield return null;
		}
		ambientSound.volume = decreaseTo;
	}

	IEnumerator _RaiseAmbientVolume(float velocity, float increaseTo){
		while(ambientSound.volume < increaseTo - 0.01f){
			ambientSound.volume += Time.deltaTime*velocity;
			yield return null;
		}
		ambientSound.volume = increaseTo;
	}


}
