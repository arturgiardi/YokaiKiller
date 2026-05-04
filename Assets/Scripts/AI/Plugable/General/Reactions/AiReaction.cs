using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiReaction : ScriptableObject 
{
	public abstract void React(StateController controller, GameObject instigator, DamageInfo damage);
	
}
