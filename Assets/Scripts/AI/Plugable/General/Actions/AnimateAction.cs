using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Animate")]
public class AnimateAction : AIAction 
{
	public float coolDown = 0;
	public string stateName;
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
