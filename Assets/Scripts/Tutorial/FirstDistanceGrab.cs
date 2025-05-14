using UnityEngine;

//Lazy script that is attached to an instantiated object that we create once we complete a part of the tutorial.
//Done because I did not want to create an assembly reference
//FIX ASAP
public class FirstDistanceGrab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("EventSystem").GetComponent<Tutorial>().FirstGrab();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
