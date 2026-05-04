using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraRenderTexture : MonoBehaviour 
{
	public Camera cineCamera;
	void OnEnable()
	{
		if(cineCamera.targetTexture != null)
		{
			cineCamera.targetTexture.Release();
		} 
		GameManager.instance.graphicsManager.AssingRT(cineCamera);
	}
	void OnDisable()
	{
		if(cineCamera.targetTexture != null)
		{
			cineCamera.targetTexture.Release();
		} 
	}

}
