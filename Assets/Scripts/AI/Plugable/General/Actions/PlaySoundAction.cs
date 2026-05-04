using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Play Sound")]
public class PlaySoundAction : AIAction 
{
	public GameObject sound;
	public override void Act(StateController controller)
	{
		PlaySound(controller);
	}	

	void PlaySound(StateController controller)
	{
		controller.GetComponent<SimpleAudioPlayer>().PlayAudio(sound);
	}

}
