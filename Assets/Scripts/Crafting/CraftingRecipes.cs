using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "New Recipe")]
public class CraftingRecipes : ScriptableObject
{
    public ItemData itemToCrafting; // El yapımı işlem sonucu üretilecek eşyanın verilerini tutar.
    public ResourceCost[] cost; // El yapımı işlem için gerekli kaynakların listesini içeren dizi.

    // Bu sınıf, el yapımı tariflerin temsilini sağlar ve ScriptableObject'ten türetilmiştir. ScriptableObject, oyun içi verileri saklamak ve yönetmek için kullanılır.
}

[System.Serializable]
public class ResourceCost
{
    public ItemData item; // Bir kaynağın verilerini temsil eder (örneğin, odun, demir cevheri).
    public int quantity; // El yapımı işlem için gereken bu kaynağın miktarını belirtir.
}
