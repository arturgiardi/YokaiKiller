using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Turn")]
public class TurnAction : AIAction 
{
	public override void Act(StateController controller)
	{
		Turn(controller);
	}	

	void Turn(StateController controller)
	{
		if(controller.cooldownTimer > 0)
			return;
		controller.transform.localScale = new Vector3(-controller.transform.localScale.x, controller.transform.localScale.y, controller.transform.localScale.z);
	}

}
