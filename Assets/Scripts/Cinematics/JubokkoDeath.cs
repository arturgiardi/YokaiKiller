using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JubokkoDeath : MonoBehaviour {
	//[SerializeField] bool triggered = false;
	[SerializeField] GameObject bossObj;
	[SerializeField] GameObject bossSprite;
	[SerializeField] ParticleSystem bossDeathParticles;
	[SerializeField] Transform initialFocus;
	[SerializeField] Animator roomBlocks;
	[SerializeField] Animator eye;

	public void RunCinematic(){
		StartCoroutine(_CinematicExecution(bossObj));
	}


	IEnumerator _CinematicExecution(GameObject boss){
		GameManager.instance.controller.DisableController();
		//AudioManager.instance.ChangeVolume(3, 0);
		roomBlocks.SetTrigger("UnBlock");
		FindObjectOfType<CameraManager>().ShakeCamera(0.5f,5f);
		yield return new WaitForSeconds(1.5f);
		FindObjectOfType<CameraManager>().ChangeFocus(bossObj.transform);
		yield return new WaitForSeconds(3f);
		bossSprite.GetComponent<SpriteRenderer>().enabled = false;
		bossDeathParticles.Play();
        //for(int i = 0; i < 20; i++)
        //{
            //bossSprite.GetComponent<DamageFeedback>().OnHit();
        //}
        
		yield return new WaitForSeconds(3f);
		FindObjectOfType<CameraManager>().ChangeFocus(GameManager.instance.controller.transform);
		GameManager.instance.controller.EnableController();
		Destroy(bossObj);
	}


	IEnumerator _LowerVolume(AudioSource source){
		while(source.volume >= 0.1f){
			source.volume = Mathf.Lerp(source.volume, 0, 5*Time.deltaTime);
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
