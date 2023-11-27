using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private ItemSlot currentSlot;
    private Outline outline;
    public int index;
    public bool equipped;


    public void Set(ItemSlot slot)
    {
        currentSlot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;

 

        if (slot.quantity > 1)
        {
            quantityText.text = slot.quantity.ToString();
        }
        else
        {
            quantityText.text = string.Empty;
        }

       
    }

    public void ClearSlot()
    {
        currentSlot = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    public void OnButtonclick()
    {
        Inventory.instance.SelectItem(index);
    }
}
