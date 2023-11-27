using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EquipTools : Equip
{
    
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public bool DoesGatherResources;
    public bool doesdealDamage;
    public int damage;
    [Header("Ranged Weapon")]   
    public bool pistolType;
    public bool assaultType;
    public GameObject muzzle;
    public Transform muzzlePoint;
    public AudioClip shotSound;
    
    private AudioSource audios;

    private Animator itemAnim;
    private Camera cam;
    public Sprite weaponSprite;

    [Header("Zoom")]
    public bool isScoped;
    private Scope scope;
    public float zoomFOV = 10f;
    public float normalFov;
    public bool isSniper;

    private void Awake()
    {
        itemAnim = GetComponent<Animator>();
        cam = Camera.main;
        audios = GetComponent<AudioSource>();       
         
    } 

    private void Start()
    { 
        PlayerPrefs.GetFloat("CurrentPistolAmmo");
        PlayerPrefs.GetFloat("CurrentAssaultAmmo");
        
    }

    private void Update()
    {
        scope = GameObject.FindObjectOfType<Scope>();
    }

    public override void OnAttackInput()
    {
        if(!attacking && !pistolType && ! assaultType)
        {
            attacking = true;
            itemAnim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
        }
        else if(!attacking && pistolType && AmmoManager.instance.curPistolAmmo > 0)
        {
            
            attacking = true;
            itemAnim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
            GameObject obj=  Instantiate(muzzle, muzzlePoint.transform.position,muzzlePoint.transform.rotation * Quaternion.Euler(90, 0, 0));
            Destroy(obj, 0.05f);
            audios.PlayOneShot(shotSound);
            
            AmmoManager.instance.curPistolAmmo --;
            PlayerPrefs.SetFloat("CurrentPistolAmmo", AmmoManager.instance.curPistolAmmo);
        }
        else if (!attacking && assaultType && AmmoManager.instance.curAssaultAmmo > 0)
        {

            attacking = true;
            itemAnim.SetTrigger("Attack");
            Invoke("OnCanAttack", attackRate);
            GameObject obj = Instantiate(muzzle, muzzlePoint.transform.position, muzzlePoint.transform.rotation * Quaternion.Euler(90, 0, 0));
            Destroy(obj, 0.05f);
            audios.PlayOneShot(shotSound);
            
            AmmoManager.instance.curAssaultAmmo--;
            PlayerPrefs.SetFloat("CurrentAssaultAmmo", AmmoManager.instance.curAssaultAmmo);
            
        }


    }

    public override void OnAltAttackInput()
    {
        //check if our weapon is sniper
        if(isSniper == true)
        {
            //is scoped will be take the oposite value
            isScoped = !isScoped;
            //activate scope image depend on our bool is scoped
            scope.scopeImage.SetActive(isScoped);
            //deactivate equip Camera so we dont see our weapon when we are zooming
            scope.weaponCamera.SetActive(!isScoped);

            if(isScoped == true)
            {
                //change the fov of main camera to zoom fov
                normalFov = scope.mainCamera.fieldOfView;
                scope.mainCamera.fieldOfView = zoomFOV;
            }
            if(isScoped == false)
            {
                //return back main camera fov to normal
                scope.mainCamera.fieldOfView = normalFov;
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        //Debug.Log("Hit");
    }
}
