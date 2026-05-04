using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Patrol")]
public class PatrolAction : AIAction 
{
	public override void Act(StateController controller)
	{
		if(!controller.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
			Patrol(controller);
	}

	private void Patrol(StateController controller)
	{
		if(controller.cooldownTimer > 0)
			return;
		controller.animator.Play("Walk",0);
		controller.animator.SetFloat("SpeedMultiplier", 1);
	}

}
