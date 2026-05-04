using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/Melee")]
public class MeleeDecision : AIDecision 
{
	public override bool Decide(StateController controller)
	{
		return InRangeForMelee(controller);
	}

	private bool InRangeForMelee(StateController controller)
	{
		Vector3 closestPoint = controller.target.GetComponent<Collider>().ClosestPointOnBounds(controller.origin.position);

		if(Vector3.Distance(closestPoint, controller.origin.position) < controller.stats.interactionRange)
		{
			return true;
		}
		return false;
	}


	
}
