using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    //static List<string> generatedPools = new List<string>();
    static Dictionary<string, List<GameObject>> poolRoot = new Dictionary<string, List<GameObject>>();

    public static GameObject PoolOut(GameObject prefab)
    {
        if(poolRoot.ContainsKey(prefab.name))
        {
            GameObject pooledObject = DragFromPool(poolRoot[prefab.name]);
            if(pooledObject == null)
                pooledObject = AddToPool(prefab);
            return pooledObject;
        }
        else
        {
            GameObject pooledObject = CreatePool(prefab);
            return pooledObject;
        }
    }


    static GameObject DragFromPool(List<GameObject> targetList)
    {
        if(!targetList[0].activeInHierarchy)
        {
            GameObject availableItem = targetList[0];
            availableItem.SetActive(true);
            targetList.Remove(availableItem);
            targetList.Add(availableItem);
            return availableItem;
        }
        else
        {
            return null;
        }
    }

    static GameObject AddToPool(GameObject prefab)
    {
        GameObject newItem = Instantiate(prefab) as GameObject;
        DontDestroyOnLoad (newItem);
        poolRoot[prefab.name].Add(newItem);

        return newItem;
    }

    static GameObject CreatePool(GameObject prefab)
    {
        GameObject newItem = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad (newItem);
        poolRoot.Add(prefab.name, new List<GameObject>());
        poolRoot[prefab.name].Add(newItem);

        return newItem;
    }

    public static void DisableAll()
    {
        foreach(KeyValuePair<string, List<GameObject>> entry in poolRoot)
        {
            for(int n = 0; n < entry.Value.Count; n++)
            {
                //DontDestroyOnLoad(entry.Value[n]);
                entry.Value[n].SetActive(false);
            }
        }
    }

}
