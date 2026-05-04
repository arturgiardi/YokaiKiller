using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactHitFeedback : MonoBehaviour 
{
	public SimpleAudioPlayer aPlayer;
	public GameObject sfx;
	void OnTriggerEnter()
	{
		aPlayer.PlayAudio(sfx);
	}
}
