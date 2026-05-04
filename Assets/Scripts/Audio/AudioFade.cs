using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioFade : MonoBehaviour {
    AudioSource audioSource;
    IEnumerator volCoroutine;

    void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeVolume(float velocity, float value)
    {
        if (value > audioSource.volume)
        {
            if (volCoroutine != null)
                StopCoroutine(volCoroutine);
            volCoroutine = _RaiseVolume(velocity, value);
            StartCoroutine(volCoroutine);
        }
        else
        {
            if (volCoroutine != null)
                StopCoroutine(volCoroutine);
            volCoroutine = _LowerVolume(velocity, value);
            StartCoroutine(volCoroutine);
        }
    }

    IEnumerator _LowerVolume(float velocity, float decreaseTo)
    {
        while (audioSource.volume > decreaseTo + 0.01f)
        {
            audioSource.volume -= Time.deltaTime * velocity;
            yield return null;
        }
        audioSource.volume = decreaseTo;
    }

    IEnumerator _RaiseVolume(float velocity, float increaseTo)
    {
        while (audioSource.volume < increaseTo - 0.01f)
        {
            audioSource.volume += Time.deltaTime * velocity;
            yield return null;
        }
        audioSource.volume = increaseTo;
    }



}
