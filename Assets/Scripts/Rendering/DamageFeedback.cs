using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedback : MonoBehaviour {

    [SerializeField] private Transform origin;
    [SerializeField] private ParticleSystem bloodParticles;
	[SerializeField] private ParticleSystem deathParticles;

	void Start()
	{
		StateController baseController = GetComponent<StateController>();
		if(baseController != null)
		{
			baseController.stats.OnDamage += OnHit;
			baseController.stats.OnDeath += OnKill;
			baseController.ResetEvent += ResetFeedback;
		}
	}

	void ResetFeedback(EnemyStats newStats)
	{
		newStats.OnDamage += OnHit;
		newStats.OnDeath += OnKill;
	}

	public void OnHit(GameObject instigator, DamageInfo damage, Stats attacker)
	{	
        bloodParticles.Play(true);
	}
	public void OnKill(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		float experience = GetComponent<StateController>().stats.experience;
	    bloodParticles.Play(true);
		deathParticles.Play(true);
		ParticleSystem.EmitParams newParams = new ParticleSystem.EmitParams();
		int thousands = 0;
		int hundreds = 0;
		int dezens = 0;
		int integers = 0;
		
		int lastingExperience = (int)experience;

		while(lastingExperience > 0)
		{
			if(lastingExperience > 1000)
			{
				lastingExperience -= 1000;
				thousands ++;
			}
			else if(lastingExperience >= 100)
			{
				lastingExperience -= 100;
				hundreds ++;
			}
			else if(lastingExperience >= 10)
			{
				lastingExperience -= 10;
				dezens ++;
			}
			else
			{
				lastingExperience --;
				integers ++;
			}

		}
		if(thousands > 0)
		{
			newParams.startSize = 0.35f;
			deathParticles.Emit(newParams,Mathf.RoundToInt(thousands));
		}
		if(hundreds > 0)
		{
			newParams.startSize = 0.2f;
			deathParticles.Emit(newParams,Mathf.RoundToInt(hundreds));
		}
		if(dezens > 0)
		{
			newParams.startSize = 0.12f;
			deathParticles.Emit(newParams,Mathf.RoundToInt(dezens));
		}
		if(integers > 0)
		{
			newParams.startSize = 0.04f;
			deathParticles.Emit(newParams,Mathf.RoundToInt(integers));
		}
		//newParams.startSize = 200;
		//deathParticles.Emit(newParams,Mathf.RoundToInt(experience/3));
		//deathParticles.Emit(Mathf.RoundToInt(experience/3));
		deathParticles.GetComponent<MonoBehaviour>().StartCoroutine(deathParticles.GetComponent<ParticleFollowTarget>().CreateSystem());
	}


}
