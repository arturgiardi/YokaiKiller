using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyNamePrompt : MonoBehaviour 
{
	public delegate void EnemyHitEvent(string eName);
	public static EnemyHitEvent OnHitEnemy;
	public TMP_Text nameTag;
	public Animator animator;

	void OnEnable()
	{
		OnHitEnemy += ChangeEnemyName;
	}
	void OnDisable()
	{
		OnHitEnemy -= ChangeEnemyName;
	}

	void ChangeEnemyName(string eName)
	{
		animator.SetTrigger("Trigger");
		nameTag.text = eName;
	}

}
