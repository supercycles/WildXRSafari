using System.Collections;
using UnityEngine;

//Script used to initialize scene without tutorial. Could probably just call this script in Tutorial.cs instead of having code dupe there
public class NoTutorial : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(FadeFromBlack(3.0f));
    }

    //Adds a slight fade effect when we enter our game scene to ease the player's eyes
    IEnumerator FadeFromBlack(float duration)
    {
        Color color = GameObject.Find("FadeFromBlack").GetComponent<MeshRenderer>().material.color;

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            //Calculate the new alpha based on the elapsed time
            float easedT = EaseInQuad(t / duration);
            float newAlpha = Mathf.Lerp(1.0f, 0.0f, easedT);
            color.a = newAlpha;

            //Apply the new alpha to the material
            color.a = newAlpha;
            GameObject.Find("FadeFromBlack").GetComponent<MeshRenderer>().material.color = color;

            //Wait for the next frame
            yield return null;
        }

        //Ensure transparency is full and then delete the object
        color.a = 0.0f;
        GameObject.Find("FadeFromBlack").GetComponent<MeshRenderer>().material.color = color;
        Destroy(GameObject.Find("FadeFromBlack"));

    }

    //Quadratic easing function (ease-in), used to increase period of opacity 
    float EaseInQuad(float t)
    {
        return t * t;
    }
}
