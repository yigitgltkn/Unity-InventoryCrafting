using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoManager : MonoBehaviour
{
    public GameObject scopeImage;
    public static AmmoManager instance;
    [Header("Ammo UI")]
    public Image icon;
    public GameObject WeaponUI;
    public TextMeshProUGUI AmmoText;
    private EquipTools equipTools;
    [Header("Ammo Capacity")]
    public float curPistolAmmo;
    public float maxPistolAmmo;
    public float curAssaultAmmo;
    public float MaxAssaultAmmo;
    public AudioClip ReloadSound;
    private AudioSource audios;

    private void Awake()
    {
        instance = this;        
    }

    private void Start()
    {
        WeaponUI.SetActive(false);
        audios = GetComponent<AudioSource>();
        curPistolAmmo = PlayerPrefs.GetFloat("CurrentPistolAmmo");
        curAssaultAmmo = PlayerPrefs.GetFloat("CurrentAssaultAmmo");
    }

    private void Update()
    {
        equipTools = GameObject.FindObjectOfType<EquipTools>();

        if(equipTools != null)
        {
            WeaponUI.SetActive(true);
            icon.sprite = equipTools.weaponSprite;
         
            if(equipTools.assaultType == true)
                AmmoText.text = ("" + curAssaultAmmo + "/" + "" + MaxAssaultAmmo).ToString();
            else if (equipTools.pistolType == true)
                AmmoText.text = ("" + curPistolAmmo + "/" + "" + maxPistolAmmo).ToString();
        }else
        {
            WeaponUI.SetActive(false);
        }
        
    }

    

    public void ReloadPistol(float amount)
    {
        curPistolAmmo += amount;
        audios.PlayOneShot(ReloadSound);
        
        if (curPistolAmmo >= maxPistolAmmo)
        {
            curPistolAmmo = maxPistolAmmo;
        }
        PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.curPistolAmmo);
    }

    public void ReloadAssault(float amount)
    {
        curAssaultAmmo += amount;
        audios.PlayOneShot(ReloadSound);
        if (curAssaultAmmo >= MaxAssaultAmmo)
        {
            curAssaultAmmo = MaxAssaultAmmo;
        }
        PlayerPrefs.SetFloat("CurrentAssaultAmmo", AmmoManager.instance.curAssaultAmmo);
    }
}
