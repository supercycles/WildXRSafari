using UnityEngine;

//Lazy script that is attached to an instantiated object that we create once we complete a part of the tutorial.
//Done because I did not want to create an assembly reference
//FIX ASAP
public class LastTeleport : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("EventSystem").GetComponent<Tutorial>().DidSomething("Now you will learn to turn the camera. Once again, use your right hand to follow the guide.");
    }
}
