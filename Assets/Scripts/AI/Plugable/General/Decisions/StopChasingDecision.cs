using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/StopChasing")]
public class StopChasingDecision : AIDecision 
{
	public override bool Decide(StateController controller)
	{
		return Detect(controller);
	}

	private bool Detect(StateController controller)
	{
		if(Vector3.Distance(controller.target.position, controller.origin.position) > controller.stats.activationRange)
		{
			return true;
		}
		else if(Mathf.Abs(controller.target.position.y - controller.origin.position.y) > controller.stats.interactionRange)
		{
			return true;
		}
		return false;
	}


	
}
