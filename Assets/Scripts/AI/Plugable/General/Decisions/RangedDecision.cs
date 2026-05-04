using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/Ranged")]
public class RangedDecision : AIDecision 
{
	public override bool Decide(StateController controller)
	{
		return InRangeForRanged(controller);
	}

	private bool InRangeForRanged(StateController controller)
	{

		if(Vector3.Distance(controller.target.position, controller.origin.position) < controller.stats.interactionRange)
		{
			return true;
		}
		return false;
	}


	
}
