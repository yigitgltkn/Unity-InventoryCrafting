using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    // Modifiye edilebilir envanter yuva arayüzleri
    public ItemSlotUI[] uiSlots;
    // Envantarda saklanan öğeler
    public ItemSlot[] slots;
    // Envanter penceresini açma ve kapama nesnesi
    public GameObject inventoryWindow;
    // Öğeyi düşüreceğiniz konum
    public Transform dropPosition;

    [Header("Seçilen Öğeler")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemdescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject dropButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    private PlayerController controller;
    private PlayerNeeds needs;

    private int curEquipIndex;
    [Header("Olaylar")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    // Singleton oluşturma
    public static Inventory instance;

    private void Awake()
    {
        // Singleton deseni uygulanıyor, bu sınıftan yalnızca bir örnek oluşturulabilir
        instance = this;

        // PlayerController bileşenine erişim sağlama
        controller = GetComponent<PlayerController>();

        // PlayerNeeds bileşenine erişim sağlama
        needs = GetComponent<PlayerNeeds>();
    }

    private void Start()
    {
        // Oyun başladığında envanter penceresini varsayılan olarak kapat
        inventoryWindow.SetActive(false);

        // UI yuvaları ile aynı sayıda envanter yuvası oluştur
        slots = new ItemSlot[uiSlots.Length];

        // Her envanter yuvasını başlat
        for (int x = 0; x < slots.Length; x++)
        {
            // Yeni bir envanter yuvası örneği oluştur
            slots[x] = new ItemSlot();
            // UI yuvasına indeksini atama
            uiSlots[x].index = x;
            // UI yuvasını temizleme
            uiSlots[x].ClearSlot();
        }

        // Seçili öğe penceresini temizleme
        ClearSelectedItemWindow();
    }

    // Envanteri açma/kapatma düğmesi için giriş işlemi
    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    // Envanteri açma veya kapatma işlevi
    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            controller.ToggleCursor(true);
        }
    }

    // Envanterin açık olup olmadığını kontrol etme işlevi
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    // Öğe eklemek için işlev
    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            // Yığınlamak için bir yuva arar
            ItemSlot slotToStackTo = GetItemSTack(item);

            // Eğer böyle bir yuva bulunursa, miktarını artır ve UI'yi güncelle
            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        // Boş bir yuva bulursa, öğeyi ekler
        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        // Ne yığınlama ne de boş bir yuva bulunmazsa, öğeyi düşürür
        ThrowItem(item);
    }

    // Öğeyi düşürmek için işlev
    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, dropPosition.rotation);
    }

    // UI'yi güncellemek için işlev
    void UpdateUI()
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item != null)
            {
                uiSlots[x].Set(slots[x]);
            }
            else
            {
                uiSlots[x].ClearSlot();
            }
        }
    }

    // Bir öğeyi yığınlayabilecek bir yuva arama işlevi
    ItemSlot GetItemSTack(ItemData item)
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item == item && slots[x].quantity < item.maxStackAmount)
            {
                return slots[x];
            }
        }
        return null;
    }

    // Boş bir yuva arama işlevi
    ItemSlot GetEmptySlot()
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item == null)
            {
                return slots[x];
            }
        }
        return null;
    }

    // Bir öğeyi seçme işlevi
    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;
        selectedItem = slots[index];
        selectedItemIndex = index;
        selectedItemName.text = selectedItem.item.ItemName;
        selectedItemdescription.text = selectedItem.item.itemDescribtion;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int x = 0; x < selectedItem.item.consumable.Length; x++)
        {
            selectedItemStatName.text += selectedItem.item.consumable[x].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumable[x].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.equipable && !uiSlots[index].equipped);
        unequipButton.SetActive(selectedItem.item.type == ItemType.equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    // Seçilen öğenin penceresini temizleme işlevi
    void ClearSelectedItemWindow()
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemdescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        dropButton.SetActive(false);
        useButton.SetActive(false);
        unequipButton.SetActive(false);
        equipButton.SetActive(false);
    }

    // Kullan düğmesine tıklama işlevi
    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int x = 0; x < selectedItem.item.consumable.Length; x++)
            {
                switch (selectedItem.item.consumable[x].type)
                {
                    case ConsumableType.health: needs.Heal(selectedItem.item.consumable[x].value); break;
                    case ConsumableType.Hunger: needs.Eat(selectedItem.item.consumable[x].value); break;
                    case ConsumableType.Thirst: needs.Drink(selectedItem.item.consumable[x].value); break;
                }
            }
        }
        RemoveSelectedItem();
    }

    // Ekip düğmesine tıklama işlevi
    public void OnEquipButton()
    {
        if (uiSlots[curEquipIndex].equipped)
        {
            Unequip(curEquipIndex);
        }

        uiSlots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        EquipManager.instance.EquipNew(selectedItem.item);
        UpdateUI();
        SelectItem(selectedItemIndex);
    }

    // Ekip çıkartma düğmesine tıklama işlevi
    public void OnUnequipButton()
    {
        Unequip(selectedItemIndex);
    }

    // Düşür düğmesine tıklama işlevi
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    // Ekip çıkartma işlevi
    private void Unequip(int index)
    {
        uiSlots[index].equipped = false;
        EquipManager.instance.UnEquip();
        UpdateUI();

        if (selectedItemIndex == index)
        {
            SelectItem(index);
        }
    }

    // Seçilen öğeyi kaldırma işlevi
    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity == 0)
        {
            if (uiSlots[selectedItemIndex].equipped == true)
                Unequip(selectedItemIndex);

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    // Belirtilen öğeyi envanterden kaldırma işlevi
    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].quantity--;
                if (slots[i].quantity == 0)
                {
                    if (uiSlots[i].equipped == true)
                        Unequip(i);

                    slots[i].item = null;
                    ClearSelectedItemWindow();
                }

                UpdateUI();
                return;
            }
        }
    }

    // Belirtilen öğenin belirtilen miktarda bulunup bulunmadığını kontrol etme işlevi
    public bool HasItem(ItemData item, int quantity)
    {
        int amount = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
                amount += slots[i].quantity;
            if (amount >= quantity)
                return true;
        }
        return false;
    }
}

public class ItemSlot
{
    public ItemData item;
    public int quantity;
}
