using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPrompt : MonoBehaviour 
{
	public TMP_Text itemName;
	public TMP_Text itemIcon;

	public void PopulatePrompt(Item item)
	{
		itemName.text = item.iName;
		itemIcon.text = "<sprite="+item.iIconID+">";
	}

	public void SelfDestroy()
	{
		Destroy(gameObject);
	}

}
