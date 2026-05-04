using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JubokkoHelper : MonoBehaviour 
{
	public Vector3 startPosition;
	public ParticleSystem bloodParticles;
	public ParticleSystem superBloodParticles;
	public ParticleSystem fireParticles;
	public ParticleSystem superFireParticles;
	public ParticleSystem eyeFireParticles;
	public StateController controller;
	public Transform focusPoint;
	public GameObject bloodSplatSound;
	public GameObject superBloodSplatSound;
	public GameObject explosionSound;
	public SimpleAudioPlayer aPlayer;
	[SerializeField] GameObject impalerPrefab;
	[SerializeField] GameObject needlePrefab;
	[SerializeField] GameObject needleClip;

	float healthPercentage = 1;

	public int availableAttacks = 1;
	public float coolDownMultiplier = 1;
	int attackRecharges;

	IEnumerator shadowCoroutine;

	void OnEnable()
	{
		startPosition = controller.transform.position;
		controller.stats.OnChangeHP += SetBossSpeed;
		controller.stats.OnDeath += Death;
		attackRecharges = 1;
	}
	void OnDisable()
	{
		controller.transform.position = startPosition;
		controller.stats.currentHealth = controller.stats.maxHealth;
		attackRecharges = 1;
		controller.enabled = false;
		controller.GetComponent<Animator>().Play("Jubokko@Hidden",0);
	}

	public void Teleport()
	{
		if (controller.target.transform.position.x > controller.origin.transform.position.x) 
		{
			transform.position = new Vector3(4f, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(-1,1,1);
			availableAttacks = attackRecharges;
		}
		else
		{
			transform.position = new Vector3(-4f, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(1,1,1);
			availableAttacks = attackRecharges;
		}
	}

	public void SpawnNeedleWave()
	{
		StartCoroutine(_SpawnNeedleWave());
	}

	IEnumerator _SpawnNeedleWave()
	{
		int counter = 0;
		Vector3 firstSpawn = Vector3.zero;
		if(transform.localScale == Vector3.one){
			firstSpawn = new Vector3(transform.position.x, transform.position.y +1, 0.295f);
			while(counter < 100){
				GameObject newNeedle = Instantiate(needlePrefab, null) as GameObject;
				newNeedle.transform.position = firstSpawn;
				firstSpawn.x += 0.25f;
                if ((float)counter % 4 == 0)
                    SoundEffectInstantiator.PlaySoundFX(needleClip, newNeedle.transform.position);
                counter ++;
				//yield return new WaitForSeconds(0.025f);
				float randomTimer = Random.Range(0.025f, 0.08f);
				yield return new WaitForSeconds(randomTimer);
			}
		}
		else{
			firstSpawn = new Vector3(transform.position.x, transform.position.y +1, 0.295f);
			while(counter < 100){
				GameObject newNeedle = Instantiate(needlePrefab, null) as GameObject;
				newNeedle.transform.position = firstSpawn;
				newNeedle.transform.localScale = new Vector3(-1,1,1);
				firstSpawn.x -= 0.25f;
                if ((float)counter % 4 == 0)
                    SoundEffectInstantiator.PlaySoundFX(needleClip, newNeedle.transform.position);
                counter ++;
				float randomTimer = Random.Range(0.025f, 0.08f);
				yield return new WaitForSeconds(randomTimer);
			}
		}
	}

	public void SpawnImpaler()
	{
		GameObject newImpaler = Instantiate(impalerPrefab, null) as GameObject;
		newImpaler.transform.position = new Vector3(controller.target.transform.position.x - 0.1f, transform.position.y - 0.22f, transform.position.z +0.05f);
		FindObjectOfType<CameraManager>().ShakeCamera(0.25f,5f);
	}

	public void SetBossSpeed(float maxHp, float currentHP)
	{
		healthPercentage = currentHP/maxHp;
		bloodParticles.Play(true);
		Debug.Log(healthPercentage);
		if(currentHP > 250)
		{
			controller.animator.SetFloat("ActionSpeed", 1);
			attackRecharges = 1;
			coolDownMultiplier = 1;
		}
		else if(healthPercentage > 110)
		{
            controller.animator.Play("Enrage_1", 3);
			controller.animator.SetFloat("ActionSpeed", 1.35f);
			attackRecharges = 2;
			coolDownMultiplier = 0.8f;
		}
		//else if(healthPercentage > 0.2f)
		//{
			//controller.animator.SetFloat("ActionSpeed", 1.5f);
			//attackRecharges = 3;
			//coolDownMultiplier = 0.65f;
		//}
		else
		{
			controller.animator.SetFloat("ActionSpeed", 1.7f);
			attackRecharges = 4;
			coolDownMultiplier = 0.55f;
		}
	}
	public void Death(GameObject instigator, DamageInfo damage, Stats attacker)
	{
        PlayerStatsController.instance.SaveState();
        GameManager.instance.volumeManager.StopMusic(1);
		InputManager.singleton.readingInput = false;
		GameManager.instance.cameraController.ChangeFocus(focusPoint, 1);
		StartCoroutine(_Death());
	}

	public IEnumerator _Death()
	{
		Time.timeScale = 0.3f;
		yield return new WaitForSeconds(1);
		Time.timeScale = 1f;
		controller.animator.Play("Death",0);
	}

	public void FireStart()
	{
		fireParticles.GetComponent<MonoBehaviour>().StartCoroutine(fireParticles.GetComponent<ParticleFollowTarget>().CreateSystem(3));
	}

	public void FireEnd()
	{
		fireParticles.Stop();
	}

	public void BloodExplosion()
	{
		RumbleController.RumbleThatShit(this,1f,1f);
		bloodParticles.Play(true);
		aPlayer.PlayAudio(bloodSplatSound);
	}

	public void SuperBloodExplosion()
	{
		aPlayer.PlayAudio(explosionSound);
		aPlayer.PlayAudio(superBloodSplatSound);
		RumbleController.RumbleThatShit(this,3f,3f);
		superBloodParticles.Play(true);
		superFireParticles.GetComponent<MonoBehaviour>().StartCoroutine(superFireParticles.GetComponent<ParticleFollowTarget>().CreateSystem(3));
	}

	public void DeathEnd()
	{
		EndEyeFire();
		InputManager.singleton.readingInput = true;	
		GameManager.instance.cameraController.ChangeFocus(GameManager.instance.controller.transform);
		PlayerStats.instance.AddXP(500);
	}

	public void OnDamage(GameObject instigator, DamageInfo damage, Stats attacker)
	{
	}

	public void ShadowCreatingCoroutineSwitch()
	{
		if(shadowCoroutine == null)
		{
			shadowCoroutine = _CreateShadows();
			StartCoroutine(shadowCoroutine);
		}
		else
		{
			StopCoroutine(shadowCoroutine);
			shadowCoroutine = null;
		}
	}

	IEnumerator _CreateShadows()
	{
		while(true)
		{
			GetComponent<CreateShadow>().InstantiateShadow(controller.transform, controller.GetComponent<SpriteRenderer>().sprite);
			yield return null;
			yield return null;
			yield return null;
		}
	}

	public void StartEyeFire()
	{
		eyeFireParticles.Play();
	}
	public void EndEyeFire()
	{
		eyeFireParticles.Stop();
	}

    public void BlinkRed()
    {
        controller.animator.Play("AttackBlink", 2);
    }

	
	

}
