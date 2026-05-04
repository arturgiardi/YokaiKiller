using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Reactions/SwitchState")]
public class SwitchStateReaction : AiReaction 
{
	public bool switchOnce;
	public bool switched;
	public bool resetCD;
	public int reactionCounter;
	public int internalReactionCounter = 0;
	public AIState state;

	public override void React(StateController controller, GameObject instigator, DamageInfo damage)
	{
		if(internalReactionCounter+1 < reactionCounter)
		{
			internalReactionCounter ++;
		}
		else
		{
			if(resetCD)
				controller.cooldownTimer = 0;
			if(!switchOnce)
				controller.currentState = state;
			else if(!switched)
			{
				controller.currentState = state;
				switched = true;
			}
			internalReactionCounter = 0;	
		}
		
	}
	
}
