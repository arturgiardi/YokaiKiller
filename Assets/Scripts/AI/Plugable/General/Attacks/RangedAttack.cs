using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Attacks/Ranged Attack")]
public class RangedAttack : AIAttack 
{
	public string attackName;
	public float coolDown;
	public ProjectileData atkData;
	public AnimationClip clip;
	public float animTime;
	public override void DoAttack(StateController controller)
	{
		controller.TurnToTarget();
		controller.animator.Play(attackName, 0, 0f);
		controller.cooldownTimer = coolDown;
		if(clip.events.Length <= 0)
		{
			AnimationEvent shootEv = new AnimationEvent();
			shootEv.functionName = ("FireProjectile");
			shootEv.time = animTime;
			shootEv.objectReferenceParameter = atkData as Object;
			clip.AddEvent(shootEv);
		}
	}
}
