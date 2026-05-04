using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticlePlayer : MonoBehaviour {


    public void PlayeParticle()
    {
        GetComponentInChildren<ParticleSystem>().Play();
    }
}
