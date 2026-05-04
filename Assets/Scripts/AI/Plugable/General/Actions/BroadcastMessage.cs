using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Broadcast Message")]
public class BroadcastMessage : AIAction 
{
	public string message;
	public override void Act(StateController controller)
	{
		Broadcast(controller);
	}

	private void Broadcast(StateController controller)
	{
		if(controller.cooldownTimer > 0)
			return;
		controller.gameObject.BroadcastMessage(message);
	}

}
