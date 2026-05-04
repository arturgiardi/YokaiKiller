using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour 
{
	public GameObject root;
	public Transform spritePivot;
	public Transform followPivot;
	public SpriteRenderer sRenderer;

	IInteractable currentInteractable;

	bool isOn = false;

	void Awake()
	{
		InputManager.OnPressB += StartInteraction;
		//InputManager.OnReleaseB += EndInteraction;
	}

	public void TurnOn()
	{
		root.SetActive(true);
		isOn = true;
	}

	public void TurnOff()
	{
		root.SetActive(false);
		isOn = false;
	}

	void Update()
	{
		if(isOn)
		{
			spritePivot.position = followPivot.position;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(isOn)
			return;
		IInteractable interactable = other.GetComponent<IInteractable>();
		if(interactable != null)
		{
			TurnOn();
			currentInteractable = interactable;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(!isOn)
			return;
		IInteractable interactable = other.GetComponent<IInteractable>();
		if(interactable != null)
		{
			TurnOff();
			currentInteractable.InteractEnd(this);
			currentInteractable = null;
		}
	}

	void StartInteraction()
	{
		if(currentInteractable == null)
			return;
		currentInteractable.InteractStart(this);
	}
	void EndInteraction()
	{
		if(currentInteractable == null)
			return;
		currentInteractable.InteractEnd(this);
	}

}
