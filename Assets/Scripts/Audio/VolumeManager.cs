using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
	[SerializeField] AudioMixer mixer;
	[SerializeField] AudioSource mainMusicPlayer;
	[SerializeField] float musicVolumeOffset = -10;
	[SerializeField] float masterVolumeOffset = 0;	
	IEnumerator masterVolumeCoroutine;
	IEnumerator musicVolumeCoroutine;

	bool musicOn = true;



	public void PlayMusic(float speed = 10)
	{
		if(musicVolumeCoroutine != null)
			StopCoroutine(musicVolumeCoroutine);
		musicVolumeCoroutine = _PlayStopMusic(true, speed);
		StartCoroutine(musicVolumeCoroutine);
		musicOn = true;
	}

	public void StopMusic(float speed = 10)
	{
		if(musicVolumeCoroutine != null)
			StopCoroutine(musicVolumeCoroutine);
		musicVolumeCoroutine = _PlayStopMusic(false, speed);
		StartCoroutine(musicVolumeCoroutine);
		musicOn = false;
	}
	

	public void ChangeMusic(AudioClip newClip, float speed = 5)
	{	
		if(newClip != mainMusicPlayer.clip || !musicOn)
		{
			if(musicVolumeCoroutine != null)
				StopCoroutine(musicVolumeCoroutine);
			musicVolumeCoroutine = _ChangeMusic(newClip, speed);
			StartCoroutine(musicVolumeCoroutine);
			musicOn = true;
		}
	}

	public void FadeMusicVolume(float toValue, float speed)
	{
		if(musicVolumeCoroutine != null)
			StopCoroutine(musicVolumeCoroutine);
		musicVolumeCoroutine = _FadeMusicVolume(toValue, speed);
		StartCoroutine(musicVolumeCoroutine);
	}

	IEnumerator _FadeMusicVolume(float toValue, float speed)
	{
		toValue = toValue + musicVolumeOffset;
		float musicVol = 0;
		mixer.GetFloat("MusicVolume", out musicVol);
		while(Mathf.Abs(musicVol - toValue) > 0.01f)
		{
			mixer.SetFloat("MusicVolume", Mathf.Lerp(musicVol, toValue, speed*Time.fixedDeltaTime));
			mixer.GetFloat("MusicVolume", out musicVol);
			yield return null;
		}
	}

	IEnumerator _ChangeMusic(AudioClip newClip, float speed)
	{
		speed *= 5;
		float musicVol = 0;
		mixer.GetFloat("MusicVolume", out musicVol);
		if(mainMusicPlayer.clip != null)
		{
			while(musicVol > -50)
			{
				mixer.SetFloat("MusicVolume", musicVol - Time.fixedDeltaTime*speed);
				mixer.GetFloat("MusicVolume", out musicVol);
				yield return null;
			}	
		}
		else
		{
			mixer.SetFloat("MusicVolume", -50);	
		}

		if(newClip != null)
		{
			mainMusicPlayer.clip = newClip;
			mainMusicPlayer.Play();
		}

		float toValue = 0 + musicVolumeOffset;
		musicVol = 0;
		mixer.GetFloat("MusicVolume", out musicVol);
		while(musicVol < toValue)
		{
			mixer.SetFloat("MusicVolume", musicVol + Time.fixedDeltaTime*speed*2);
			mixer.GetFloat("MusicVolume", out musicVol);
			yield return null;
		}
		mixer.SetFloat("MusicVolume", toValue);
	}

	IEnumerator _PlayStopMusic(bool play, float speed)
	{
		float musicVol = 0 + musicVolumeOffset;
		if(play)
		{
			mainMusicPlayer.Play();
			mixer.GetFloat("MusicVolume", out musicVol);
			while(Mathf.Abs(musicVol - 0) > 0.01f)
			{		
				mixer.SetFloat("MusicVolume", Mathf.Lerp(musicVol, 0, speed*Time.fixedDeltaTime));
				mixer.GetFloat("MusicVolume", out musicVol);
				yield return null;
			}
		}
		else
		{
			mixer.GetFloat("MusicVolume", out musicVol);
			while(Mathf.Abs(musicVol - -40) > 0.01f)
			{	
				mixer.SetFloat("MusicVolume", Mathf.Lerp(musicVol, -40, speed*Time.fixedDeltaTime));
				mixer.GetFloat("MusicVolume", out musicVol);
				yield return null;
			}
			mainMusicPlayer.Stop();	
		}
		
	}	

	public void FadeMasterOff()
	{
		if(masterVolumeCoroutine != null)
			StopCoroutine(masterVolumeCoroutine);
		masterVolumeCoroutine = _FadeMaster(-90, 40);
		StartCoroutine(masterVolumeCoroutine);
	}

	public void FadeMasterOn()
	{
		if(masterVolumeCoroutine != null)
			StopCoroutine(masterVolumeCoroutine);
		masterVolumeCoroutine = _FadeMaster(0, 40);
		StartCoroutine(masterVolumeCoroutine);
	}

	IEnumerator _FadeMaster(float volume, float speed)
	{
		float targetVolume = volume + masterVolumeOffset;
		float currentVolume = 0;
		mixer.GetFloat("MasterVolume", out currentVolume);
		if(targetVolume < currentVolume)
		{
			while(currentVolume > targetVolume)
			{
				
				mixer.SetFloat("MasterVolume", currentVolume - Time.unscaledDeltaTime * speed);
				mixer.GetFloat("MasterVolume", out currentVolume);
				yield return null;
			}
		}
		else
		{
			
			while(currentVolume < targetVolume)
			{
				
				mixer.SetFloat("MasterVolume", currentVolume + Time.unscaledDeltaTime * speed);
				mixer.GetFloat("MasterVolume", out currentVolume);
				yield return null;
			}
		}
		
		mixer.SetFloat("MasterVolume", targetVolume);
	}

}
