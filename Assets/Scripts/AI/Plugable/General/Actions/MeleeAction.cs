using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Melee")]
public class MeleeAction : AIAction 
{
	public override void Act(StateController controller)
	{
		Melee(controller);
	}

	private void Melee(StateController controller)
	{
		controller.Attack();
	}

}
