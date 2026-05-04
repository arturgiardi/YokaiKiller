using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject prefab;
	GameObject instance;

	float direction;
	

	void Awake()
	{
		direction = transform.GetChild(0).localScale.x;
		Destroy(transform.GetChild(0).gameObject);
	}

	void OnEnable()
	{
		if(instance != null)
			Destroy(instance);
		instance = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
		instance.transform.localScale = new Vector3(direction, instance.transform.localScale.y, instance.transform.localScale.z);
	}
	void OnDisable()
	{
		if(instance != null)
			Destroy(instance);
	}

}
