using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/LinearDetect")]
public class LinearDetectDecision : AIDecision 
{

	public LayerMask layerCheck;
	public override bool Decide(StateController controller)
	{
		return Detect(controller);
	}

	private bool Detect(StateController controller)
	{
		if(Vector3.Distance(controller.target.position, controller.origin.position) < controller.stats.activationRange)
		{
			Ray[] rayArray =	
			{
				new Ray(controller.origin.position, (controller.target.position) - controller.origin.position)
			};
			Debug.DrawRay(rayArray[0].origin, rayArray[0].direction);
			foreach(Ray ray in rayArray)
			{
				if(Physics.Raycast(ray, Vector3.Distance(controller.origin.position, controller.target.position), layerCheck))
				{
					return false;
				}
				else
					return true;
			}
		}
		return false;
	}


	
}
