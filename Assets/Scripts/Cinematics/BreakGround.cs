using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BreakGround : MonoBehaviour
{
	public GameObject deactive;
	public GameObject active;
	public GameObject[] fragments;
	public GameObject raikoFragment;
	public float delay;


	public IEnumerator Break(Transform raiko)
	{
		deactive.SetActive(false);
		active.SetActive(true);

		yield return new WaitForSeconds(1.2f);
		foreach (GameObject fragment in fragments)
		{
			fragment.GetComponentInChildren<ParticleSystem>().Play();
			fragment.transform.DOMoveY(fragment.transform.position.y - 10f, 1f).SetEase(Ease.InQuad);
			yield return new WaitForSeconds(delay);
		}
	}
}
