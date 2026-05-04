using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Look At Target")]
public class LookAtAction : AIAction 
{
	public override void Act(StateController controller)
	{
		LookAtTarget(controller);
	}	

	void LookAtTarget(StateController controller)
	{
		controller.TurnToTarget();
	}

}
