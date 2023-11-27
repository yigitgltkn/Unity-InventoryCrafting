using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingRecipeUI : MonoBehaviour
{
    public CraftingRecipes recipe; // Bu arayüzün bağlı olduğu el yapımı tarif.
    public Image backgroundImage; // Arkaplan resmi.
    public Image icon; // El yapımı eşyanın simgesi.
    public TextMeshProUGUI itemName; // El yapımı eşyanın adı.
    public Image[] resourceCosts; // Gerekli kaynakların listesi (görsel).

    public Color canCraftColor, cannotCraftColor; // El yapımı işlemin yapılabilir ve yapılamaz durumları için arka plan rengi.
    private bool canCraft; // El yapımı işlemin yapılabilirliğini tutan bir değişken.

    private void Start()
    {
        icon.sprite = recipe.itemToCrafting.icon; // El yapımı eşyanın simgesini ayarlar.
        itemName.text = recipe.itemToCrafting.ItemName; // El yapımı eşyanın adını ayarlar.

        for (int i = 0; i < resourceCosts.Length; i++)
        {
            if (i < recipe.cost.Length)
            {
                resourceCosts[i].gameObject.SetActive(true); // Gerekli kaynağı etkinleştirir.
                resourceCosts[i].sprite = recipe.cost[i].item.icon; // Gerekli kaynağın simgesini ayarlar.
                resourceCosts[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = recipe.cost[i].quantity.ToString(); // Gerekli kaynağın miktarını ayarlar.
            }
            else
            {
                resourceCosts[i].gameObject.SetActive(false); // Eğer kaynaklar tükenmişse görünürlüğü kapatır.
            }
        }
    }

    private void OnEnable()
    {
        UpdateCanCraft(); // Bu arayüz etkin olduğunda, yapılabilirlik durumunu günceller.
    }

    public void UpdateCanCraft()
    {
        canCraft = true;
        // Kaynakları döngü içinde kontrol eder.
        for (int i = 0; i < recipe.cost.Length; i++)
        {
            // Envantrende yeterli kaynağın olup olmadığını kontrol eder.
            if (!Inventory.instance.HasItem(recipe.cost[i].item, recipe.cost[i].quantity))
            {
                canCraft = false;
                break;
            }
        }

        // Arkaplan rengini, el yapımı işlemin yapılabilirliğine bağlı olarak günceller.
        backgroundImage.color = canCraft ? canCraftColor : cannotCraftColor;
    }

    public void OnClickButton()
    {
        if (canCraft)
        {
            CraftingWindow.instance.Craft(recipe); // El yapımı işlemi yapılabilirse, bu işlemi başlatır.
        }
    }
}
