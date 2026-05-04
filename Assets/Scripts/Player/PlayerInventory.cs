using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour 
{
	public List<Item> inventory;

	public List<Item> filterInventory(ItemType type)
	{
		List<Item> filteredITems = new List<Item>();
		foreach(Item item in inventory)
		{
			if(item.iType == type)
			{
				filteredITems.Add(item);
			}
		}
		return filteredITems;
	}

	public void AddItem(Item item)
	{
		if(!item.stackable)
			inventory.Add(item);
		else
		{
			if(!StackItem(item))
			{
				item.stack = 1;
				inventory.Add(item);
			}
		}
	}

	public void RemoveItem(Item item)
	{
		inventory.Remove(item);
	}

	bool StackItem(Item item)
	{
		foreach(Item invItem in inventory)
		{
			if(invItem.id == item.id)
			{
				if((invItem.stack + 1) < invItem.maxStack)
				{
					invItem.stack++;
					return true;
				}
			}
		}
		return false;
	}

}
