using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remover : MonoBehaviour {

    public void DestroyThis(){
        Destroy(this.gameObject);
    }
}
