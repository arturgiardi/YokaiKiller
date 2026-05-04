using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMessage : MonoBehaviour 
{

	[System.Serializable]
	public class MessageContent{
		public float[] numbers;
		public Vector3[] vectors;
		public string [] strings;

		public MessageContent(float[] numbers, Vector3[] vectors, string[] strings)
		{
			this.numbers = numbers;
			this.vectors = vectors;
			this.strings = strings;
		}

	}

    [SerializeField]
    private GameObject target;
    [SerializeField]
    private string message;
	[SerializeField]
	private MessageContent messageContent;
	[SerializeField]
	private bool singeton= false;

	void Start(){
		
		if (singeton) {
			target = GameManager.instance.gameObject;
		}
	}

    void OnTriggerEnter(Collider other){
		target.SendMessage(message, messageContent, SendMessageOptions.DontRequireReceiver);
		//this.GetComponent<Collider> ().enabled = false;
    }

	public string[] GetStrings()
	{
		return messageContent.strings;
	}
	
}
