using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public delegate void OnInventoryAction(Item item);
    public static OnInventoryAction OnItemGet;

    public GameObject itemCardPrefab;
    public Transform inventoryGrid;
    public Transform cardHolder;
    public List<GameObject> cardPool;

    public Text itemDescField;

    public void RenderItemList()
    {
    }

    public List<GameObject> GetAllItemCards()
    {
        //PoolCardsIn();
        List<GameObject> returnCards = new List<GameObject>();
        foreach(Item item in GameManager.instance.playerInventory.inventory)
        {
            if(item.iType != ItemType.Artifact && item.iType != ItemType.Weapon)
            returnCards.Add(PoolCardOut(item));
        }
        return returnCards;
    }

    public List<GameObject> GetWeaponCards()
    {
        //PoolCardsIn();
        List<GameObject> returnCards = new List<GameObject>();
        foreach(Item item in GameManager.instance.playerInventory.inventory)
        {
            if(item.iType == ItemType.Weapon)
                returnCards.Add(PoolCardOut(item));
        }
        return returnCards;
    }
    public List<GameObject> GetArtifactCards()
    {
         List<GameObject> returnCards = new List<GameObject>();
        foreach(Item item in GameManager.instance.playerInventory.inventory)
        {
            if(item.iType == ItemType.Artifact)
            {
                returnCards.Add(PoolCardOut(item));
            }
        }
        return returnCards;
    }
    public void PoolCardsIn()
    {
        foreach(GameObject card in cardPool)
        {
            card.SetActive(false);
            card.transform.SetParent(cardHolder, false);
        }
    }

    public void PoolInSingleCard(GameObject card)
    {
        card.SetActive(false);
        card.transform.SetParent(cardHolder, false);
    }

    GameObject PoolCardOut(Item item)
    {
        foreach(GameObject card in cardPool)
        {
            if(!card.activeInHierarchy)
            {
                card.GetComponent<ItemCard>().PopulateCard(item);
                card.SetActive(true);
                return card;
            }
        }
        GameObject newCard = Instantiate(itemCardPrefab) as GameObject;
        newCard.GetComponent<ItemCard>().PopulateCard(item);
        cardPool.Add(newCard);

        return newCard;
    }

    public void DisplayItemDescription(string desc)
    {
        itemDescField.text = desc;
    }



}
