using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour 
{
	//IDs para demo:
	//0: akaiitou sala 3.3
	//1: scroll do poder
	//2: scroll de vida
	//3: segundo scroll de vida
	public int id;
	public GameObject prefab;
	GameObject instance;

	void Awake()
	{
		Destroy(transform.GetChild(0).gameObject);
	}

	void OnEnable()
	{
		if(instance != null)
			Destroy(instance);
		if(!GameManager.instance.itemValidator.obtainedIds.Contains(id))
		{
			//Debug.Log("Item id: " + id + " was not obtained yet!");
			instance = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
		}
		else
		{
			//Debug.Log("Item id: " + id + " was already obtained!");
		}
	}
	void OnDisable()
	{
		if(instance != null)
			Destroy(instance);
	}

}
