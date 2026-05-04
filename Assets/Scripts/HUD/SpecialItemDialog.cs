using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpecialItemDialog : MonoBehaviour 
{

	public static SpecialItemDialog instance;

	bool spawned;
	[SerializeField]
	TMP_Text itemName;
	[SerializeField]
	TMP_Text itemDescription;
	[SerializeField]
	Animator animator;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (gameObject);
	}

	void Update()
	{
		if (spawned) 
		{
			if (Input.anyKeyDown) 
			{
				spawned = false;
				animator.SetTrigger ("Hide");
			}
		}
	}

	public void SpanText(string displayName, string displayText)
	{
		spawned = true;
		itemName.text = displayName;
		itemDescription.text = displayText;
		animator.SetTrigger ("Show");
		Time.timeScale = 0;
		GameManager.instance.SetGameState(GameState.Paused);
	}

	public void ReleaseTime ()
	{
		StartCoroutine(_ReleaseTime());
	}

	IEnumerator _ReleaseTime()
	{
		Time.timeScale = 1;
		yield return null;
		GameManager.instance.SetGameState(GameState.Playing);
	}

	



}
