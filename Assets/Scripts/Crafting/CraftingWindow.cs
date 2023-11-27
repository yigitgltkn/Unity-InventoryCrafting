using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    public static CraftingWindow instance; // Tek bir örnek oluşturulmasını sağlamak için bir örnek (Singleton deseni).
    public CraftingRecipeUI[] recipeUIs; // El yapımı tariflerin UI öğelerini depolayan dizi.
    public GameObject inventoryPanel; // Envateri temsil eden oyun nesnesi.

    private void Awake()
    {
        instance = this; // Singleton örneğini oluşturur ve ayarlar.
    }

    private void OnEnable()
    {
        // Envaterin açılma olayına bir dinleyici ekler.
        Inventory.instance.onOpenInventory.AddListener(OnOpenInventory);
    }

    private void OnDisable()
    {
        // Envaterin açılma olayından dinleyiciyi kaldırır.
        Inventory.instance.onOpenInventory.RemoveListener(OnOpenInventory);
    }

    public void OnOpenInventory()
    {
        gameObject.SetActive(false); // Bu pencereyi gizler.
        inventoryPanel.SetActive(true); // Envater panelini gösterir.
    }

    public void OnOpenCraft()
    {
        gameObject.SetActive(true); // El yapımı penceresini gösterir.
        inventoryPanel.SetActive(false); // Envater panelini gizler.
        Cursor.lockState = CursorLockMode.None; // Fareyi serbest bırakır.
        PlayerController.instance.canLook = false; // Oyuncunun bakma yeteneğini devre dışı bırakır.
    }

    public void OnCloseCraft()
    {
        gameObject.SetActive(false); // El yapımı penceresini kapatır.
        Cursor.lockState = CursorLockMode.Locked; // Fareyi kilitler.
        PlayerController.instance.canLook = true; // Oyuncunun bakma yeteneğini etkinleştirir.
    }

    public void Craft(CraftingRecipes recipe)
    {
        // Tarif için gerekli envanter öğelerini kaldırmak için döngü.
        for (int i = 0; i < recipe.cost.Length; i++)
        {
            // Miktarlarına göre envanter öğelerini kaldırma döngüsü.
            for (int x = 0; x < recipe.cost[i].quantity; x++)
            {
                Inventory.instance.RemoveItem(recipe.cost[i].item);
            }
        }

        // El yapımı öğeyi envantere ekler.
        Inventory.instance.AddItem(recipe.itemToCrafting);

        // El yapımı tariflerin UI'sini güncellemek için döngü.
        for (int i = 0; i < recipeUIs.Length; i++)
        {
            recipeUIs[i].UpdateCanCraft();
        }
    }
}
