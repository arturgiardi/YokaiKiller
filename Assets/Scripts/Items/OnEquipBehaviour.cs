using UnityEngine;

public abstract class OnEquipBehaviour : ScriptableObject
{
    public string effectName;
    public bool stackable;
    public int priority;
    public abstract void EquipItem(PlayerStats stats, Item item);
    public abstract void RemoveItem(PlayerStats stats, Item item);
}