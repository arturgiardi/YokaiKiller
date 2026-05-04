using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStartRandomizer : MonoBehaviour 
{

	AudioSource source;
	[SerializeField] float minPitch;
	[SerializeField] float maxPitch;
	
	void OnEnable () 
	{
		if(!GetComponent<AudioSource>())
			return;
		source = GetComponent<AudioSource>();
		source.pitch = Random.Range(minPitch, maxPitch);
		source.PlayScheduled((double)Random.Range(0, source.clip.length));
	}
	
}
