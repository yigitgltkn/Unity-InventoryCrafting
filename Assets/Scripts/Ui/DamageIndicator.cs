using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image damageImage;
    public float flashSpeed;
    private Coroutine fadeAwayImage;

    public void Flashing()
    {
        //if image exist then stop coroutine
        if (fadeAwayImage != null)
            StopCoroutine(fadeAwayImage);
        //activate the image
        damageImage.enabled = true;
        //put the image alpha to white(See the image)
        damageImage.color = Color.white;
        //call couroutine function
        fadeAwayImage = StartCoroutine(FadeAwayImage());
    }


    IEnumerator FadeAwayImage()
    {
        //
        float imageAlpha = 1.0f;

        //loop through image alpha and if its 0 then
        while(imageAlpha > 0.0f)
        {
            //change the alpha back slowly
            imageAlpha -= (1.0f / flashSpeed) * Time.deltaTime;
            damageImage.color = new Color(1.0f, 1.0f, 1.0f, imageAlpha);
            yield return null;
        }

        damageImage.enabled = false;
    }
}
