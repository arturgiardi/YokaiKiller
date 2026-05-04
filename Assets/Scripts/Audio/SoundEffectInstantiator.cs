using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectInstantiator : MonoBehaviour 
{

    public static GameObject PlaySoundFX(GameObject prefab, Vector3 position)
    {
        if(prefab != null)
        {
            GameObject newSound =  Instantiate(prefab, position, Quaternion.identity);
            return newSound;
        }
        return null;
    }

}
