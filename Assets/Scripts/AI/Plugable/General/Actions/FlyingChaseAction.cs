using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/FlyingChase")]
public class FlyingChaseAction : AIAction 
{

	public float speed;
	//float currentSpeed = 0;
	Vector3 direction;

	public override void Act(StateController controller)
	{
		Chase(controller);
	}

	private void Chase(StateController controller)
	{
		if(controller.cooldownTimer > 0 )
			return;

		//Vector3 direction = (controller.target.position - controller.transform.position).normalized;
		controller.animator.Play("Chase",0);
		if(direction == Vector3.zero)
			direction = controller.target.position;
		direction = Vector3.Lerp(direction, controller.target.position, 2*Time.deltaTime);
		controller.transform.position = Vector3.MoveTowards(controller.transform.position, direction, speed*Time.deltaTime);
	}

}
