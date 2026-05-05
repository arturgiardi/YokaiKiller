using UnityEngine;

public class PlatformDownInputController : MonoBehaviour
{
	[SerializeField]
	private Transform reference;
	[SerializeField]
	private PlatformColliderSwitch platformCollider;
	private CharacterController body;
	void Start () {
		//reference = GameManager.instance.controller.transform;
		reference = FindObjectOfType<NewController>().transform;
		body = reference.GetComponent<CharacterController> ();
	}

    private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);
	}


}
