using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/Detect")]
public class DetectDecision : AIDecision 
{

	public LayerMask layerCheck;
	
	public override bool Decide(StateController controller)
	{
		return Detect(controller);
	}

	private bool Detect(StateController controller)
	{
		//Debug.Log(Mathf.Abs(controller.target.position.y - controller.origin.position.y) < controller.stats.activationHeight);
		if(Vector3.Distance(controller.target.position, controller.origin.position) < controller.stats.activationRange)
		{
			if(Mathf.Abs(controller.target.position.y - controller.origin.position.y) < controller.stats.activationHeight)
			{
				Ray[] rayArray =	
				{
					new Ray(controller.origin.position, (controller.target.position) - controller.origin.position)
				};
				Debug.DrawRay(rayArray[0].origin, rayArray[0].direction);
				RaycastHit hit = new RaycastHit();
				foreach(Ray ray in rayArray)
				{
					if(Physics.Raycast(ray, out hit, Vector3.Distance(controller.origin.position, controller.target.position), layerCheck))
					{
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			else
			{
				return false;
			}
		}
		return false;
	}


	
}
