using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class GraphicsManager_OLD : MonoBehaviour {
	public Toggle forwardPath;
	public Toggle deferredPath;
	public Toggle grassHigh;
	public Toggle grassMed;
	public Toggle grassLow;
	public Toggle grassOff;
	public Toggle shadowUltra;
	public Toggle shadowHigh;
	public Toggle shadowMed;
	public Toggle shadowLow;
	public Toggle shadowOff;
	public Toggle bloom;
	public Toggle dof;

	public GameObject gg1;
	public GameObject gg2;
	public GameObject gg3;


	void Start(){
		
	}

	public void tooglePath(int path){
		if(path == 0){
			Camera.main.renderingPath = RenderingPath.DeferredShading;
		}
		else{
			Camera.main.renderingPath = RenderingPath.Forward;
		}
	}

	public void toogleGrass(int ammount){
		if(ammount == 0){
			gg1.SetActive(false);
			gg2.SetActive(false);
			gg3.SetActive(false);
		}
		else if(ammount == 1){
			gg1.SetActive(true);
			gg2.SetActive(false);
			gg3.SetActive(false);
		}
		else if(ammount == 2){
			gg1.SetActive(true);
			gg2.SetActive(true);
			gg3.SetActive(false);
		}
		else if(ammount == 3){
			gg1.SetActive(true);
			gg2.SetActive(true);
			gg3.SetActive(true);
		}
	}

	public void toogleShadow(int value){
		QualitySettings.SetQualityLevel(value);
	}

	public void toogleBloom(bool value){
		FindObjectOfType<Bloom>().enabled = value;
	}
	public void toogleDof(bool value){
		FindObjectOfType<DepthOfField>().enabled = value;
	}


}
