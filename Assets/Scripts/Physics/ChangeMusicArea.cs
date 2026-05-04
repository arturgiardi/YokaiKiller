using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicArea : MonoBehaviour 
{

	public bool stop;
	public float speed;
	GameObject target;
	public AudioClip newMusic;

	void Start(){
		
		target = GameManager.instance.gameObject;
	}

    void OnTriggerEnter(Collider other){
		//if(newMusic != null)
		//{
			if(stop)
				GameManager.instance.volumeManager.StopMusic((speed == 0 ? 10 : speed));
			else
				GameManager.instance.volumeManager.ChangeMusic(newMusic, (speed == 0 ? 10 : speed));
			//this.GetComponent<Collider> ().enabled = false;
		//}
		//else
		//{
			//if(stop)
				//GameManager.instance.volumeManager.StopMusic((speed == 0 ? 10 : speed));
			//else
				//GameManager.instance.volumeManager.PlayMusic((speed == 0 ? 10 : speed));
			//this.GetComponent<Collider> ().enabled = false;
		//}
		
    }
	
}
