using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bu enum, oyun öğelerinin türlerini temsil eder.
public enum ItemType
{
    equipable,   // Giyilebilir öğeler (örneğin, zırh veya silah)
    Consumable,  // Tüketilebilir öğeler (örneğin, yiyecek veya içecek)
    Resource     // Kaynak öğeler (örneğin, odun veya taş)
}

// Bu enum, tüketilebilir öğelerin türlerini temsil eder.
public enum ConsumableType
{
    Hunger,      // Açlık
    Thirst,      // Susuzluk
    health,      // Sağlık
    Battery      // Pil (örneğin, el feneri için)
}

// ScriptableObject, Unity içinde büyük miktarda veri saklamak için kullanılan bir veri konteyneridir.
// Bir ScriptableObject, sınıf örneklerinin bellek kullanımını azaltmak amacıyla projede bağımsız olarak kullanılabilir.
// Bu özellikle bir projede bir prefab gibi birden çok yerde kullanılabilen veriler varsa faydalıdır.

// [CreateAssetMenu] niteliği, bu sınıfın Unity editöründe yaratılabilen bir varlık (asset) olmasını sağlar.
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    // "Information" başlığı altındaki öğe bilgileri
    public string ItemName;      // Öğe adı
    public string itemDescribtion; // Öğe açıklaması
    public ItemType type;         // Öğe türü (Equipable, Consumable, Resource)
    public Sprite icon;           // Öğenin simgesi (ikon)
    public GameObject dropPrefab; // Öğenin sahnede düşürülüğünde görünen prefab'ı

    // "Stacking Items" başlığı altındaki öğe yığılabilirlik (stackability) özellikleri
    public bool canStack;         // Öğelerin yığılabilir olup olmadığını belirtir.
    public int maxStackAmount;    // Maksimum yığılabilir öğe miktarı

    // "Consumable" başlığı altındaki tüketilebilir öğe özellikleri
    public ItemDataConsumable[] consumable; // Tüketilebilir öğenin türü ve değeri

    // "Equip Items" başlığı altındaki giyilebilir öğe özellikleri
    public GameObject equipPrefab; // Giyilebilir öğenin prefab'ı
}

// Bu sınıf, tüketilebilir öğelerin türünü ve değerini tutar.
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; // Tüketilebilir öğenin türü (Hunger, Thirst, Health, Battery, vb.)
    public float value;         // Tüketilebilir öğenin etkisinin miktarı veya değeri
}
