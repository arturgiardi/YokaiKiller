using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.
using TMPro;

public class ItemCard : MonoBehaviour,  ISelectHandler
{
	public delegate void PickItemEvent(Item item);
	public static PickItemEvent onPickItem;
	public TextMeshProUGUI nameLabel;
	public TextMeshProUGUI iconLabel;
	public Item itemReference;
	public string customName;
	public string customSelectionString;


	void Awake()
	{
		if(itemReference == null)
		{
			itemReference = ScriptableObject.CreateInstance<Item>();
			itemReference.iDesc = customSelectionString;
			itemReference.iName = customName;
			itemReference.id = -1;
		}
	}

	public void PopulateCard(Item item)
	{
		nameLabel.text = item.iName;
		iconLabel.text = "<sprite="+item.iIconID+">";
		itemReference = item;
	}

	public void OnSelect(BaseEventData eventData)
    {
        //GameManager.instance.inventoryManager.DisplayItemDescription(itemReference.iDesc);
		if(itemReference != null)
		{
			if(ItemDescName.OnItemSelect != null)
				ItemDescName.OnItemSelect(itemReference);
			if(ItemStatsPredict.OnItemSelect != null && (itemReference.iType==ItemType.Weapon || itemReference.iType==ItemType.Artifact))
				ItemStatsPredict.OnItemSelect(itemReference);
			else
			{
				ItemStatsPredict.OnItemSelect(null);
			}
		}
		else
		{
			ItemStatsPredict.OnItemSelect(null);
		}
		
    }

	public void OnPickItem()
	{
		if(onPickItem != null)
			onPickItem(itemReference);
	}
}


