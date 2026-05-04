using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFXFeedback : MonoBehaviour 
{
	public static PlayerFXFeedback instance;

	public ParticleSystem helingFX;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else
			Destroy(this);
	}

	public void Heal()
	{
		helingFX.Play();
	}

}
