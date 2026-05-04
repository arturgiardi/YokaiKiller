using UnityEngine;
using System.Collections;
 



[ExecuteInEditMode]
public class SpecialCameraFX : MonoBehaviour 
{
 
 public float scrolling;
 public Material material;
 
 
void Start()
{
	GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
}


 // Postprocess the image
 void OnRenderImage (RenderTexture source, RenderTexture destination)
 {
 	//material.SetFloat("_scrolling", scrolling);
 	Graphics.Blit (source, destination, material);
 }
}