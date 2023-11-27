using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerNeeds : MonoBehaviour,IDamageable
{
    public Need health;
    public Need hunger;
    public Need thirst;
    
    public float NoHungerHPDecay;
    public float noThirstHPDecay;
    public UnityEvent getDamage;
    private void Start()
    {
        //current value is equal to the start value
        health.currentValue = health.startValue;
        hunger.currentValue = hunger.startValue;
        thirst.currentValue = thirst.startValue;
        
    }

    private void Update()
    {
        //reduce the value over time
        hunger.Subtract(hunger.decayRate * Time.deltaTime);
        thirst.Subtract(thirst.decayRate * Time.deltaTime);

        // check if hunger bar reach zero then
        if(hunger.currentValue == 0.0f)
        {
            //reduce health depend on the value of noHungerHPDecay
            health.Subtract(NoHungerHPDecay * Time.deltaTime);
        }

        if (thirst.currentValue == 0.0f)
        {
            health.Subtract(noThirstHPDecay * Time.deltaTime);
        }

        //check if player health reached to zero then call Die function
        if(health.currentValue == 0.0f)
        {
            Die();
        }

        

        //update  sliders 
        health.uiSlider.fillAmount = health.GetPercentage();
        hunger.uiSlider.fillAmount = hunger.GetPercentage();
        thirst.uiSlider.fillAmount = thirst.GetPercentage();
    }

    public void Heal(float amount)
    {
        //add to health depend on that amount
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        thirst.Add(amount);
    }

    public void TakeDamage(int damageAmount)
    {
        //reduce health depend on damage amount
        health.Subtract(damageAmount);
        //if we get damage then invoke 
        getDamage?.Invoke();
    }

    public void Die()
    {
        
        Debug.Log("Player Died");
    }
}

[System.Serializable]
public class Need
{
    public float currentValue, maxValue, startValue, regenerateRate, decayRate;
    public Image uiSlider;

    public void Add(float amount)
    {
        //current value is the minimum value of 2 number 
        currentValue = Mathf.Min(currentValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        //current value is the maximum value of 2 number
        currentValue = Mathf.Max(currentValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        //return us the result of divid between current value and max value
        return currentValue / maxValue;
    }
}

public interface IDamageable
{
    void TakeDamage(int DamageAmount);
}
