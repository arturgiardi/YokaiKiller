using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Plugable AI/Attacks/Flying Charge Attack")]
public class FlyingChargeAttack : AIAttack 
{
	public string attackName;
	public float coolDown;
	public float chargeSpeed;
	public GameObject prepareSound;
	public GameObject attackSound;
	public LayerMask layerCheck;
	public override void DoAttack(StateController controller)
	{
		controller.TurnToTarget();
		controller.GetComponent<MonoBehaviour>().StartCoroutine(_DoAttack(controller));
		controller.cooldownTimer += coolDown;
	}

	IEnumerator _DoAttack(StateController controller)
	{
		if(controller.stats.alive)
			controller.GetComponent<SimpleAudioPlayer>().PlayAudio(prepareSound);
		yield return new WaitForSeconds(coolDown/3);
		if(controller.stats.alive)
			controller.GetComponent<SimpleAudioPlayer>().PlayAudio(prepareSound);
		Ray ray = new Ray(controller.origin.position, ((controller.target.position) - controller.origin.position).normalized);
		RaycastHit hit = new RaycastHit();
		Vector3 targetPosition = Vector3.zero;
		if(Physics.Raycast(ray, out hit, 3, layerCheck))
		{
			targetPosition = controller.transform.position + ray.direction * (Vector3.Distance(controller.transform.position, hit.point) - 0.5f);
		}
		else
		{
			targetPosition = controller.transform.position + ray.direction * 3f;
		}
		if(controller.target.position.x > controller.origin.position.x && controller.transform.localScale.x < 0)
		{	
			controller.transform.localScale = new Vector3(1, controller.transform.localScale.y, controller.transform.localScale.z);
		}
		else if(controller.target.position.x < controller.origin.position.x)
		{
			controller.transform.localScale = new Vector3(-1, controller.transform.localScale.y, controller.transform.localScale.z);
		}
		controller.animator.Play("AttackBlink",2);
		yield return new WaitForSeconds(coolDown/6);
		if(controller.stats.alive)
			controller.GetComponent<SimpleAudioPlayer>().PlayAudio(attackSound);
		while(controller.cooldownTimer  > 0)
		{
			if(!controller.stats.alive)
				yield break;
			controller.transform.position = Vector3.MoveTowards(controller.transform.position, targetPosition, chargeSpeed*Time.deltaTime);
			yield return null;
		}
	}

}
