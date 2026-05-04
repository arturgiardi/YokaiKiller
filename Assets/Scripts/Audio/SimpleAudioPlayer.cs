using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioPlayer : MonoBehaviour {

	public GameObject PlayAudio(GameObject clip)
    {
        return SoundEffectInstantiator.PlaySoundFX(clip, transform.position);
    }
}
