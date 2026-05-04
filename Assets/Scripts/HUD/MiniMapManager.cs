using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour 
{

	[SerializeField] Transform minimapParent;
	[SerializeField] Image currentRoom;
	
	void OnEnable()
	{
		SceneSwitchTest.OnSwitchRoom += OnAccessRoom;
	}
	void OnDisable()
	{
		SceneSwitchTest.OnSwitchRoom -= OnAccessRoom;
	}

	void OnAccessRoom(string roomName)
	{
		if(currentRoom != null)
			currentRoom.color = Color.white;
		//print("Searching " + roomName + " in minimap");
		for(int i = 0; i < minimapParent.childCount; i++)
		{
			//print(minimapParent.GetChild(i).name);
			if(minimapParent.GetChild(i).name == roomName)
			{
				//print("Found " + roomName + " in minimap!");
				minimapParent.GetChild(i).GetComponent<Image>().enabled = true;
				minimapParent.localPosition = -1*minimapParent.GetChild(i).localPosition;
				currentRoom = minimapParent.GetChild(i).GetComponent<Image>();
				currentRoom.color = Color.magenta;
			}
		}
	}

}
