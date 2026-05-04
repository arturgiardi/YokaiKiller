using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/State")]
public class AIState : ScriptableObject 
{

	public AIAction[] actions;
	public AIStateTransition[] transitions;

	public void UpdateState(StateController controller)
	{
		DoActions(controller);
		CheckTransitions(controller);
	}
	
	private void DoActions(StateController controller)
	{
		for(int i = 0; i < actions.Length; i++)
		{
			actions[i].Act(controller);
		}
	}

	private void CheckTransitions(StateController controller)
	{
		for(int i = 0; i < transitions.Length; i++)
		{
			bool decisionSucceded = transitions[i].decision.Decide(controller);
			if(decisionSucceded)
			{
				if(controller.TransitionToState(transitions[i].trueState))
				{
					return;
				}
			}
			else
			{
				if(controller.TransitionToState(transitions[i].falseState))
				{
					return;
				}
			}
		}
	}

}
