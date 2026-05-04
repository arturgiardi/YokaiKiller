using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteShadow : MonoBehaviour {
	public Renderer render;
	// Use this for initialization
	void Start () {
		render = this.GetComponent<Renderer>();
		render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        render.receiveShadows = true;
        //render.material.renderQueue = -1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
