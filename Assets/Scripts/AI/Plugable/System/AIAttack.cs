using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAttack : ScriptableObject
{
	public abstract void DoAttack(StateController controller);
}
