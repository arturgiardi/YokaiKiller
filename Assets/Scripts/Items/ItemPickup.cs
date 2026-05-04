using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public ParticleSystem ps;
    public SimpleAudioPlayer ap;
    public GameObject pickupSound;
    public int obtainableId = -1;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        item = Object.Instantiate(item) as Item;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.playerInventory != null)
        {
            //->Executar evento de obtenção de item
            GameManager.instance.playerInventory.AddItem(item);
            if(InventoryManager.OnItemGet != null)
                InventoryManager.OnItemGet(item);
            if(ps != null)
                ps.Play();
            if(ap != null && pickupSound != null)
                ap.PlayAudio(pickupSound);
            if(obtainableId != -1)
            {
                GameManager.instance.itemValidator.obtainedIds.Add(obtainableId);
            }
            gameObject.SetActive(false);
        }
    }

}