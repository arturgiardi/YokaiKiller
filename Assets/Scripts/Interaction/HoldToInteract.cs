using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HoldToInteract : MonoBehaviour, IInteractable
{
    [HideInInspector]
    public float triggerPercentage;

    [SerializeField]
    float interactionTriggerDelay = 2;

    float interactionTime;
    IEnumerator delayCoroutine;
    InteractionManager currentManager;

    public void InteractStart(InteractionManager manager)
    {
        TriggerInteraction();
        manager.TurnOff();
        //if(delayCoroutine != null)
            //StopCoroutine(delayCoroutine);
        //delayCoroutine = _InteractionDelay();
        //StartCoroutine(delayCoroutine);
    }
    public void InteractEnd(InteractionManager manager)
    {
        if(delayCoroutine != null)
            StopCoroutine(delayCoroutine);
    }


    IEnumerator _InteractionDelay()
    {
        while(triggerPercentage < 1)
        {
            interactionTime += Time.deltaTime;
            triggerPercentage = interactionTime/interactionTriggerDelay;
            yield return null;
        }
        triggerPercentage = 1;
        InteractEnd(currentManager);
        TriggerInteraction();
    }

    public abstract void TriggerInteraction();
}
