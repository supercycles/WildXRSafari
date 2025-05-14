using UnityEngine;

//Lazy script that is attached to an instantiated object that we create once we complete a part of the tutorial.
//Done because I did not want to create an assembly reference
//FIX ASAP

public class TeleportedOnce : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("EventSystem").GetComponent<Tutorial>().DidSomething("Great! Try teleporting two more times.");
    }
}
