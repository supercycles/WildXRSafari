using UnityEngine;

//Are we starting with the tutorial?
public class TutorialCheck : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
