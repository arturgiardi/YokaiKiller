using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JubokkoIntro : MonoBehaviour {
	[SerializeField]
	bool triggered = false;
	bool fast = false;
	bool finished = false;
	[SerializeField]
	StateController boss;
	[SerializeField]
	Transform initialFocus;
	[SerializeField]
	Transform eyeFocus;
	[SerializeField]
	Animator roomBlocks;
	[SerializeField]
	Animator eye;
    [SerializeField]
    AudioClip bossThemeMusic;

	void OnTriggerEnter()
	{
		if(finished)
			return;
		eyeFocus = GameObject.FindWithTag("Script Target").transform;
		boss = GameObject.FindWithTag("Boss").GetComponent<StateController>();
		boss.GetComponent<StateController>().stats.OnDeath += LowerWalls;
		if(!triggered && !fast)
		{
			GameObject musicPlayer = GameObject.FindWithTag("Game Music");
			CameraManager cameraMan = FindObjectOfType<CameraManager>().GetComponent<CameraManager>();
			StartCoroutine(_CinematicExecution(musicPlayer.GetComponent<AudioSource>(), cameraMan));
			triggered = true;
		}
		else if (!triggered && fast)
		{
			GameObject musicPlayer = GameObject.FindWithTag("Game Music");
			CameraManager cameraMan = FindObjectOfType<CameraManager>().GetComponent<CameraManager>();
			StartCoroutine(_ShortCinematicExecution(musicPlayer.GetComponent<AudioSource>(), cameraMan));
			triggered = true;
		}
	}

	void OnDisable()
	{
		if(!finished)
		{
			roomBlocks.Play("null",0);
			boss.GetComponent<StateController>().stats.OnDeath -= LowerWalls;
		}
		
	}

	void OnEnable()
	{
		if(!finished)
			triggered = false;
	}

	


	IEnumerator _CinematicExecution(AudioSource source, CameraManager cameraMan){
        //GameManager.instance.controller.DisableController();
        CinematicController.Cinematic_DisableIngameHud();
		InputManager.singleton.readingInput = false;
		roomBlocks.SetTrigger("Block");
		cameraMan.ShakeCamera(0.5f,5f);
		yield return new WaitForSeconds(3);
		GameManager.instance.volumeManager.StopMusic(0.5f);
		cameraMan.ChangeFocus(initialFocus, 1);
		yield return new WaitForSeconds(1f);
		eye.SetTrigger("BlinkEye");
		yield return new WaitForSeconds(4f);
		cameraMan.ChangeFocus(eyeFocus, 1);
		//cameraMan.ChangeFocus(boss.transform, 1);
        //GameManager.instance.hudManager.UpdateBossHP(boss.stats.maxHealth, boss.stats.currentHealth);
        //GameManager.instance.hudManager.ShowBossHP();
        //yield return new WaitForSeconds(0.5f);
		boss.GetComponent<Animator>().Play("BattleIntro", 0);
		yield return new WaitForSeconds(1f);
		GameManager.instance.volumeManager.PlayMusic();
		GameManager.instance.volumeManager.ChangeMusic(bossThemeMusic, 10);
		//AudioManager.instance.SetMusic(bossThemeMusic, true);
		//AudioManager.instance.ChangeVolume(0.1f, 1);
		yield return new WaitForSeconds(2f);
		cameraMan.ChangeFocus(GameManager.instance.controller.transform);
		yield return new WaitForSeconds(1f);
		//GameManager.instance.controller.EnableController();
		InputManager.singleton.readingInput = true;
		boss.GetComponent<StateController>().enabled = true;
		fast = true;
        CinematicController.Cinematic_EnableIngameHud();
        //bossObj.SendMessage("StartCombat");
    }

	IEnumerator _ShortCinematicExecution(AudioSource source, CameraManager cameraMan){
        CinematicController.Cinematic_DisableIngameHud();
        //GameManager.instance.controller.DisableController();
        InputManager.singleton.readingInput = false;
		roomBlocks.SetTrigger("Block");
		cameraMan.ShakeCamera(0.5f,5f);
		yield return new WaitForSeconds(3);
		GameManager.instance.volumeManager.StopMusic(0.5f);
		cameraMan.ChangeFocus(eyeFocus, 1);
		boss.GetComponent<Animator>().Play("BattleIntro", 0);
		yield return new WaitForSeconds(1f);
		GameManager.instance.volumeManager.PlayMusic();
		GameManager.instance.volumeManager.ChangeMusic(bossThemeMusic, 10);
		yield return new WaitForSeconds(2f);
		cameraMan.ChangeFocus(GameManager.instance.controller.transform);
		yield return new WaitForSeconds(1f);
		InputManager.singleton.readingInput = true;
		boss.GetComponent<StateController>().enabled = true;
        CinematicController.Cinematic_EnableIngameHud();
    }

	void LowerWalls(GameObject instigator, DamageInfo damage, Stats attacker)
	{
		CameraManager cameraMan = FindObjectOfType<CameraManager>().GetComponent<CameraManager>();
		roomBlocks.SetTrigger("Unblock");
		cameraMan.ShakeCamera(0.5f,5f);
		finished = true;
	}

	IEnumerator _LowerVolume(AudioSource source){
		while(source.volume >= 0.1f){
			source.volume = Mathf.Lerp(source.volume, 0, 0.4f*Time.deltaTime);
			yield return null;
		}
		source.volume = 0;
	}
	IEnumerator _RaiseVolume(AudioSource source){
		while(source.volume <= 0.9f){
			source.volume = Mathf.Lerp(source.volume, 1, 5*Time.deltaTime);
			yield return null;
		}
		source.volume = 1;
	}

}
