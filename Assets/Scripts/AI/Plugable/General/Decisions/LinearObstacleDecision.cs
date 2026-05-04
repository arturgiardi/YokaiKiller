using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/LinearObstacle")]
public class LinearObstacleDecision : AIDecision 
{
	public LayerMask layerCheck;
	public override bool Decide(StateController controller)
	{
		return CheckObstacles(controller);
	}

	bool CheckObstacles(StateController controller)
	{
		Ray[] rayArray =	
		{
			new Ray(controller.origin.position, (controller.target.position) - controller.origin.position)
		};
		foreach(Ray ray in rayArray)
		{
			if(Physics.Raycast(ray, controller.stats.wallDetectionRange, layerCheck))
			{
				return true;
			}
		}
		return false;
	}
}
