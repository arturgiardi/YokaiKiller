using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainRenderer : MonoBehaviour {
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] Transform[] chainPoints;
	Vector3[] positions = new Vector3[6];
	
	// Update is called once per frame
	void Update () {
		positions[0] = chainPoints[0].position;
		positions[1] = chainPoints[1].position;
		positions[2] = chainPoints[2].position;
		positions[3] = chainPoints[3].position;
		positions[4] = chainPoints[4].position;
		positions[5] = chainPoints[5].position;
		lineRenderer.SetPositions(positions);
	}
}
