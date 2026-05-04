using UnityEngine;
using System.Collections;

public abstract class SubweaponBase : DamageInfo
{
    public delegate void SubweaponCallback();
    public int icon;
    public float cooldownTime;
    public GameObject dropPrefab;
    public ComboSetup combo;
    public abstract void EvaluateCombo(NewController controller, SubweaponCallback callback, NewController.ComboState comboState);
    public abstract void ApplySpecialEffect(StateController target);
    public abstract DamageInfo GenerateSubweaponDamage();
}
