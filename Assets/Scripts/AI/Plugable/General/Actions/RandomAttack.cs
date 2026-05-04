using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Actions/RandomAttack")]
public class RandomAttack : AIAction 
{
	public AIAttack[] possibleAttacks;
	public override void Act(StateController controller)
	{
		PickRandomAttack(controller);
	}

	private void PickRandomAttack(StateController controller)
	{
		int randomAttack = Random.Range(0, possibleAttacks.Length);
		controller.attack = possibleAttacks[randomAttack];
		controller.Attack();
	}

}
