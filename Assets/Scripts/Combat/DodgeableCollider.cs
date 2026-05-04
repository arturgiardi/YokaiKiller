using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeableCollider : MonoBehaviour 
{
    public Bounds bounds;
	Collider thisCollider;
    Transform reference;
    CharacterController body;
    bool dodging;


    void Start()
    {
        //reference = GameManager.instance.controller.transform;
        reference = GameManager.instance.controller.transform;
        body = reference.GetComponent<CharacterController>();

    }

    void OnEnable()
	{
        thisCollider = GetComponent<Collider>();
		if(thisCollider != null)
		{
            DodgeReactor.OnDodgeStart -= DisableCollider;
            DodgeReactor.OnDodgeEnd -= EnableCollider;
            DodgeReactor.OnDodgeStart += DisableCollider;
			DodgeReactor.OnDodgeEnd += EnableCollider;
		}
	}

	void OnDestroy()
	{
		if(thisCollider != null)
		{
            DodgeReactor.OnDodgeStart -= DisableCollider;
            DodgeReactor.OnDodgeEnd -= EnableCollider;
        }
	}


	void DisableCollider()
	{
        dodging = true;
        thisCollider.enabled = false;
	}

	void EnableCollider()
	{
        dodging = false;
        thisCollider.enabled = true;
	}

    void Update()
    {
        if (dodging)
            return;
        
        Vector3 footPosition = (reference.position + body.center);
        footPosition.y -= (body.height / 2);
        float topPosition = thisCollider.transform.position.y + ((bounds.extents.y-0.1f) * thisCollider.transform.lossyScale.y) + bounds.center.y;
        //print (colliderObject.size);
        /* if (!thisCollider.enabled)
        {
            if (footPosition.y >= topPosition + 0.1f)
            {
                thisCollider.enabled = false;
            }
            else
            {
                thisCollider.enabled = true;
            }
        }*/
        //else
        //{
            if (footPosition.y >= topPosition - 0.15f)
            {

                thisCollider.enabled = false;
            }
            else
            {
                thisCollider.enabled = true;
            }
        // }
        //if (refa != null && refb != null) {
        //	refa.position = footPosition;
        //	refb.position = new Vector3(transform.position.x, topPosition, transform.position.z);
        //}
    }

}
