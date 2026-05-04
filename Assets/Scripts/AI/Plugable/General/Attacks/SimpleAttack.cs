using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Attacks/Simple Attack")]
public class SimpleAttack : AIAttack 
{
	public string attackName;
	public float coolDown;
	public override void DoAttack(StateController controller)
	{
		controller.TurnToTarget();
		controller.animator.Play(attackName, 0, 0f);
		controller.cooldownTimer = coolDown;
	}
}
