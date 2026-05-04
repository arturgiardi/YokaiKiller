using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoBozuShieldSwitch : MonoBehaviour
{
	public Animator shieldAnimator;
	public SimpleAudioPlayer aPlayer;
	public GameObject shieldOnSFX;
	bool shieldOn = false;

	public void TurnShieldOn()
	{
		if(!shieldOn)
		{
			shieldAnimator.SetBool("On", true);
			aPlayer.PlayAudio(shieldOnSFX);	
			shieldOn = true;
		}	
	}

	public void TurnShieldOff()
	{
		if(shieldOn)
		{
			shieldAnimator.SetBool("On", false);
			shieldOn = false;
		}
		
	}

}
