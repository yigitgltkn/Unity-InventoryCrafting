using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour,IInteractable
{
    public ItemData item;


    // IInteractable arayüzünün gerektirdiği GetInteractPrompt fonksiyonu.
    public string GetInteractPromp()
    {
        // Nesnenin üzerine işaretçi (crosshair) geldiğinde eşyanın adını gösterir.
        return string.Format("pickup {0}", item.ItemName);
    }

    // IInteractable arayüzünün gerektirdiği OnInteract fonksiyonu.
    public void OnInteract()
    {
        // Eşyayı envantere ekler.
        Inventory.instance.AddItem(item);
        // Nesneyle etkileşim sonrasında nesneyi yok eder.
        Destroy(gameObject);
    }
}
