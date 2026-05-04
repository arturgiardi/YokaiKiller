using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Move")]
public class MoveAction : AIAction 
{
	public float coolDown = 0;
	public string stateName;
	public Vector3 movement;
	public override void Act(StateController controller)
	{
		Animate(controller);
	}	

	void Animate(StateController controller)
	{
		if(controller.cooldownTimer > 0)
			return;
		controller.animator.Play(stateName, 0);
		if(coolDown > 0)
			controller.cooldownTimer += coolDown;
	}

}
