using UnityEngine;

public class FirstClose : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("EventSystem").GetComponent<Tutorial>().FirstClose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
