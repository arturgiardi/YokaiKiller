using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/Behind")]
public class BehindDecision : AIDecision 
{
	public override bool Decide(StateController controller)
	{
		return DetectBehind(controller);
	}

	private bool DetectBehind(StateController controller)
	{
		if(controller.transform.localScale.x > 0 && controller.target.position.x < controller.transform.position.x)
		{
			return true;
		}
		if(controller.transform.localScale.x < 0 && controller.target.position.x > controller.transform.position.x)
		{
			return true;
		}
		return false;
	}


	
}
