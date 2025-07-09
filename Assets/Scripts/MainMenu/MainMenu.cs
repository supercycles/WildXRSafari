//using System.Collections;
//using UnityEngine;

//Currently unused, but might return to make better choices with UI positioning on initialization of app

//public class MainMenu : MonoBehaviour
//{
//    public GameObject tutorialBtn;
//    public GameObject noTutorialBtn;
//    float posX, posY, posZ;

//    void Start()
//    {
//        StartCoroutine(SetMenuPos());
//    }

//    IEnumerator SetMenuPos()
//    {
//        yield return new WaitForSeconds(1);

//        posX = tutorialBtn.transform.position.x;
//        posY = GameObject.Find("RightEyeAnchor").transform.position.y * .9f;
//        posZ = tutorialBtn.transform.position.z;

//        tutorialBtn.transform.position = new Vector3(posX, posY, posZ);
//        noTutorialBtn.transform.position = new Vector3(posX, posY - 0.08f, posZ);
//    }

//}
