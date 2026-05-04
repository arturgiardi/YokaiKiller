using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Chase")]
public class ChaseAction : AIAction 
{

	public float speedMultiplier;

	public override void Act(StateController controller)
	{
		Chase(controller);
	}

	private void Chase(StateController controller)
	{
		if(controller.cooldownTimer > 0)
			return;

		if(Mathf.Abs(controller.target.position.x - controller.origin.position.x) < controller.stats.interactionRange/2)
		{
			controller.animator.Play("Wait",0);
		}
		else
		{
			controller.animator.Play("Walk",0);
			controller.animator.SetFloat("SpeedMultiplier", speedMultiplier);
		}
	
		controller.TurnToTarget();
		
	}

}
