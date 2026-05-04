using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Decisions/Jubokko/RepeatAttack")]
public class JubokkoRepeatAttackDecision : AIDecision 
{
	public override bool Decide(StateController controller)
	{
		return AttackAgain(controller);
	}

	private bool AttackAgain(StateController controller)
	{
		JubokkoHelper helper = controller.GetComponent<JubokkoHelper>();
		if(helper.availableAttacks > 0)
		{
			helper.availableAttacks -= 1;
			return true;
		}
		else
			return false;
	}


	
}
