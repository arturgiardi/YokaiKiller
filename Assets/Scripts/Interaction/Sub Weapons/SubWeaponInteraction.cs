using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubWeaponInteraction : HoldToInteract, IInteractable
{
    [SerializeField] SubweaponBase subweapon = default;
    [SerializeField] GameObject rootGO;
    [SerializeField] Rigidbody rb;

    public void SpawnSubWeapon()
    {
        rb.AddForce(Vector3.up*2, ForceMode.Impulse);
    }
    public override void TriggerInteraction()
    {
        PlayerStatsController.instance.DropSW();
        PlayerStatsController.instance.currentSubweapon = subweapon;
        HealthGuiManager.instance.UpdateSubweaponIcon();
        Destroy(rootGO);
        //InteractionManager.EndInteraction();
    }
}
