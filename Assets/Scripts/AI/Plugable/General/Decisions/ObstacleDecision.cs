using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/Obstacle")]
public class ObstacleDecision : AIDecision 
{
	public bool checkLeap;
	public bool checkWall;
	public LayerMask layerCheck;
	public override bool Decide(StateController controller)
	{
		return CheckObstacles(controller);
	}

	bool CheckObstacles(StateController controller)
	{
		if(checkLeap)
		{
			Ray ray = new Ray(controller.origin.position, (controller.origin.position + Vector3.right*controller.stats.leapRange*controller.transform.localScale.x + Vector3.down*controller.stats.leapDepth) - controller.origin.position);
			Debug.DrawRay(ray.origin, ray.direction.normalized*((controller.stats.leapRange+controller.stats.leapDepth)), Color.green);
			if(!Physics.Raycast(ray, ((controller.stats.leapRange+controller.stats.leapDepth)), layerCheck))
			{
				return true;
			}	
		}
		if(checkWall)
		{
			// Ray[] rayArray =	{ new Ray(controller.origin.position + Vector3.up/5, (controller.origin.position + Vector3.up/5 + Vector3.right*controller.transform.localScale.x*controller.stats.wallDetectionRange) - (controller.origin.position + Vector3.up/5)) , 
			// 						new Ray(controller.origin.position, (controller.origin.position + Vector3.right*controller.transform.localScale.x*controller.stats.wallDetectionRange) - controller.origin.position),
			// 						new Ray(controller.origin.position - Vector3.up/5, (controller.origin.position - Vector3.up/5 + Vector3.right*controller.transform.localScale.x*controller.stats.wallDetectionRange) - (controller.origin.position - Vector3.up/5) )};
			// foreach(Ray ray in rayArray)
			// {
			// 	if(Physics.Raycast(ray, controller.stats.wallDetectionRange, layerCheck))
			// 	{
			// 		return true;
			// 	}
			// }
			Ray ray = new Ray(controller.origin.position, (controller.origin.position + Vector3.right*controller.transform.localScale.x*controller.stats.wallDetectionRange) - controller.origin.position);
			if(Physics.Raycast(ray, controller.stats.wallDetectionRange, layerCheck))
			{
			 		return true;
			}
		}

		return false;

	}
}
