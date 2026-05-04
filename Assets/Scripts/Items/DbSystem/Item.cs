using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Trash, Consumable, Weapon, Artifact};
[System.Serializable] public class ItemStats 
{
	public DamageInfo damage;
	public DefenseInfo defense;
}
[CreateAssetMenu (menuName = "Item")]
public class Item : ScriptableObject {

	public int id;
	public string iName;
	public string iDesc;
	public ItemType iType;
	public int iIconID;
	public Sprite iIcon;
	public ItemStats iStats;
	public bool stackable = false;
	public int stack;
	public int maxStack;

	public OnEquipBehaviour equipBehaviour;

	public void Init(string name, int id)
	{
		this.id = id;
		iName = name;
	}


	public virtual string OnEquip()
	{
		#if UNITY_EDITOR
    		Debug.Log("Equiped: " + iName);
  		#endif
		return "Equiped";
	}
	public virtual string OnUnequip()
	{
		#if UNITY_EDITOR
    		Debug.Log("Unequiped: " + iName);
  		#endif
		return "Unequiped";
	}
	public virtual string OnUse()
	{
		#if UNITY_EDITOR
    		Debug.Log("Used: " + iName);
  		#endif
		return "Used";
	}
	public virtual string OnConsume()
	{
		#if UNITY_EDITOR
    		Debug.Log("Consumed: " + iName);
  		#endif
		return "Consumed";
	}
	public virtual string OnLoot()
	{
		#if UNITY_EDITOR
    		Debug.Log("Looted: " + iName);
  		#endif
		return "Looted";
	}
	public virtual string OnActivation()
	{
		#if UNITY_EDITOR
    		Debug.Log("Activated: " + iName);
  		#endif
		return "Activated";
	}
	public virtual string OnInspect()
	{
		#if UNITY_EDITOR
    		Debug.Log("Inspected: " + iName);
  		#endif
		return "Inspected";
	}
	

}
