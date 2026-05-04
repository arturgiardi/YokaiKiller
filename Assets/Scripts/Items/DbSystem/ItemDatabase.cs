using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ItemDatabase")]
public class ItemDatabase : ScriptableObject 
{
	public List<Item> items;
	public int lastEntry = 0;
	

	public bool SetLastEntry(int entry)
	{
		if(entry > lastEntry)
		{
			lastEntry = entry;
			return true;
		}
		return false;	
	}

}
