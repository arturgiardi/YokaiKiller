using UnityEngine;
using TMPro;

public class ItemDescName : MonoBehaviour
{
    public delegate void ItemDescNameEvent (Item itemData);

    public static ItemDescNameEvent OnItemSelect;

    public TMP_Text nameField;
    public TMP_Text descField;


    void Awake()
    {
        OnItemSelect += ShowItemDescName;
    }

    void ShowItemDescName(Item data)
    {
        if(data == null)
        {
            nameField.text = "Remover equipamento";
            descField.text = "";
        }
        else
        {
            nameField.text = data.iName;
            descField.text = data.iDesc;
        }
    }


}