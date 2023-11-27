using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    public int damage;
    public float damageRate;
    private List<IDamageable> thingsToGetDamage = new List<IDamageable>();

    private void Start()
    {
        StartCoroutine(DoDamage());
    }

    //do damage with delay
    IEnumerator DoDamage()
    {
        while(true)
        {
            //loop through to the things that going to get damage
            for(int i= 0; i< thingsToGetDamage.Count; i++)
            {
                //deal damage to them
                thingsToGetDamage[i].TakeDamage(damage);
            }

            //do damage with delay of our damage rate value
            yield return new WaitForSeconds(damageRate);
        }

        
    }

    //as soon as the object collide 
    private void OnCollisionEnter(Collision collision)
    {
        //if object that have IDamageable then
        if(collision.gameObject.GetComponent<IDamageable>() != null)
        {
            //add that object to list to do damage to them
            thingsToGetDamage.Add(collision.gameObject.GetComponent<IDamageable>());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<IDamageable>() != null)
        {
            //remove the object from the list and stop damaging them
            thingsToGetDamage.Remove(collision.gameObject.GetComponent<IDamageable>());
        }
    }
}
