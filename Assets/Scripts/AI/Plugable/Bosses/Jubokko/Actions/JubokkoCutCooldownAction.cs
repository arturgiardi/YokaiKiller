using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/Jubokko/CutCooldownAction")]
public class JubokkoCutCooldownAction : AIAction 
{

	public override void Act(StateController controller)
	{
		CutCooldown(controller);
	}	

	void CutCooldown(StateController controller)
	{
		JubokkoHelper helper = controller.GetComponent<JubokkoHelper>();
		if(helper.availableAttacks == 0)
		{
			helper.availableAttacks --;
			controller.cooldownTimer = 2 * controller.GetComponent<JubokkoHelper>().coolDownMultiplier;
		}
		else
		{
			controller.cooldownTimer *= controller.GetComponent<JubokkoHelper>().coolDownMultiplier;
		}
		
	}

}
