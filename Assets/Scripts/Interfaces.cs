using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable<G,D,S>
{
    void Damage(G instigator, D damage, S attacker);
    StateController GetStateController();
    bool Living();
}
