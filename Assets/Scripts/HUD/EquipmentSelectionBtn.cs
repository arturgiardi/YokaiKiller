using UnityEngine;
using TMPro;

public class EquipmentSelectionBtn : MonoBehaviour
{
    public delegate void WeaponChangeEvent(Item item);
    public delegate void ArtifactChangeEvent(Item item, int slot);
    public static WeaponChangeEvent OnChangeWeapon;
    public static ArtifactChangeEvent OnChangeArtifact;

    public bool weapon = false;
    public int artifactSlot = 0;

    public TMP_Text itemName;
    public TMP_Text itemIcon;

    void Start()
    {
        if(weapon)
            OnChangeWeapon += ReactToWeaponChange;
        else
            OnChangeArtifact += ReactToArtifactChange;
    }

    void ReactToWeaponChange(Item item)
    {
        if(itemName != null)
            itemName.text = item.iName;
        if(itemIcon != null)
            itemIcon.text = "<sprite="+item.iIconID+">";
        //Debug.Log("weapon changed");
    }

    void ReactToArtifactChange(Item item, int slot)
    {
        //Debug.Log("Artifact changed");
        if(item != null)
        {
            if(slot == artifactSlot)
            {
                if(item.id == -1)
                {
                    if(itemName != null)
                        itemName.text = "";
                    if(itemIcon != null)
                        itemIcon.text = "";
                }
                else
                {
                    if(itemName != null)
                        itemName.text = item.iName;
                    if(itemIcon != null)
                        itemIcon.text = "<sprite="+item.iIconID+">";
                }
                
            }
        }
        else
        {
            if(slot == artifactSlot)
            {
                if(itemName != null)
                    itemName.text = "";
                if(itemIcon != null)
                    itemIcon.text = "";
            }
        }
    }
}