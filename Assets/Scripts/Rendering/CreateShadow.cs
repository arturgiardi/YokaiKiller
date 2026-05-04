using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShadow : MonoBehaviour {
	public GameObject shadowPrefab;
	public Transform spawnPoint;

	public SpriteRenderer spriteRenderer;
	public Transform refference;

	IEnumerator coroutine;

	public void InstantiateShadow(Transform refference, Sprite sprite)
	{
		GameObject go = Instantiate(shadowPrefab, null, false) as GameObject;
		go.transform.position = spawnPoint.position + Vector3.down/6;
		go.transform.localScale = refference.localScale;
		go.GetComponent<SpriteRenderer>().sprite = sprite;
	}

	public void AutoCreateShadow()
	{
		GameObject go = Instantiate(shadowPrefab, null, false) as GameObject;
		go.transform.position = spawnPoint.position;
		go.transform.localScale = refference.localScale;
		go.GetComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
	}
	/*public void SwitchShadowCoroutine(Transform refference, Sprite sprite)
	{
		if(coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		else
		{
			coroutine = _CreatingShadows(refference, sprite);
			StartCoroutine(coroutine);
		}
	}

	IEnumerator _CreatingShadows(Transform refference, Sprite sprite)
	{
		while(true)
		{
			GameObject go = Instantiate(shadowPrefab, null, false) as GameObject;
			go.transform.position = spawnPoint.position + Vector3.down/6;
			go.transform.localScale = refference.localScale;
			go.GetComponent<SpriteRenderer>().sprite = sprite;
		}
	}*/

}
