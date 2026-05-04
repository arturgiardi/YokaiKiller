using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour 
{
	bool isInside = false;
	bool isGrounded = false;
	bool isActive = false;
	bool interacted = false;

	void OnEnable()
	{
        isInside = false;
        isGrounded = false;
        isActive = false;
        interacted = false;

        InputManager.OnPressX += StartSaving;
	}

	void OnDisable()
	{
		InputManager.OnPressX -= StartSaving;
	}

	void Update()
	{
        if (interacted)
            return;
		if(isInside && isGrounded && !isActive && !interacted)
		{
			GameManager.instance.interactionManager.TurnOn();
			isActive=true;
		}
		if(isActive && !isGrounded)
		{
			GameManager.instance.interactionManager.TurnOff();
			isActive=false;
		}
		if(interacted)
		{
			GameManager.instance.interactionManager.TurnOff();
			isActive=false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
        if (interacted)
            return;
		NewController controller = other.GetComponent<NewController>();
		if(controller != null)
		{
			isInside = true;
			isGrounded = controller.CheckGrounded();
		}
	}
	void OnTriggerExit(Collider other)
	{
        if (interacted)
            return;
        NewController controller = other.GetComponent<NewController>();
		if(controller != null)
		{
			isInside = false;
			isGrounded = false;
			isActive = false;
			interacted = false;
			GameManager.instance.interactionManager.TurnOff();
		}
	}
	void OnTriggerStay(Collider other)
	{
        if (interacted)
            return;
        NewController controller = other.GetComponent<NewController>();
		if(controller != null)
		{
			isGrounded = controller.CheckGrounded();
		}
	}


	void StartSaving()
	{
		if(isActive)
		{
            GameManager.instance.interactionManager.TurnOff();
            interacted = true;
			SendMessage("Interact");
		}
	}
	
}
