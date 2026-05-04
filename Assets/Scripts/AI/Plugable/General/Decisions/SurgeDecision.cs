using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/Surge")]
public class SurgeDecision : AIDecision 
{
	public override bool Decide(StateController controller)
	{
		return CheckTarget(controller);
	}

	bool CheckTarget(StateController controller)
	{
		if(Vector3.Distance(controller.target.position, controller.origin.position) < controller.stats.activationRange 
			&& Vector3.Distance(controller.target.position, controller.origin.position) > controller.stats.activationRange/10f)
		{
			if(Mathf.Abs(controller.target.position.y - controller.origin.position.y) < controller.stats.activationHeight)
			{
				if(controller.transform.localScale.x > 0 && controller.target.position.x > controller.transform.position.x)
					return true;
				else if(controller.transform.localScale.x < 0 && controller.target.position.x < controller.transform.position.x)
					return true;
			}
			else
				return false;
		}
		return false;
	}
}
