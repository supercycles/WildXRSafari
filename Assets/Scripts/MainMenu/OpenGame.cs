using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//Handles the opening of our game scene from the main menu, whether or not we start with the tutorial
//It should be asked if I should have two separate scenes for the tutorial/nontutorial, rather than just having some sort of universal bool that we check in the scripts that need it
public class OpenGame : MonoBehaviour
{
    public GameObject noTutorialConfirmation;
    float posX, posY, posZ;

    //If we press the "Start Game" button, start the game with the tutorial. Disable menu ui except for version and display loading text.
    public void StartWithTutorial()
    {
        GameObject.Find("LoadText").GetComponent<TextMeshProUGUI>().enabled = true;
        GameObject.Find("StartGameBtn").SetActive(false);
        GameObject.Find("StartGameNoTutorialBtn").SetActive(false);
        GameObject.Find("Logo").SetActive(false);
        StartCoroutine(LoadGame(true));
    }

    //If we press the "Start Game without Tutorial" button, we are given a warning asking the user to confirm if they want to skip the tutorial
    public void StartWithoutTutorialWarning()
    {
        noTutorialConfirmation.SetActive(true);
        Vector3 vHeadPos = Camera.main.transform.position;
        Vector3 vGazeDir = Camera.main.transform.forward;
        noTutorialConfirmation.transform.position = (vHeadPos + vGazeDir * .45f) + new Vector3(0.0f, -.40f, 0.0f);
        Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0; vRot.x = 0;
        noTutorialConfirmation.transform.eulerAngles = vRot;
    }

    //If we confirm that we want to skip the tutorial after the warning, disable menu ui and display loading text, prepare to load non tutorial scene.
    public void StartWithoutTutorial()
    {
        GameObject.Find("LoadText").GetComponent<TextMeshProUGUI>().enabled = true;
        if (noTutorialConfirmation != null) { noTutorialConfirmation.SetActive(false); }
        GameObject.Find("Logo").SetActive(false);
        StartCoroutine(LoadGame(false));
    }

    //If we heed the skip tutorial warning, we are returned to the base main menu
    public void CancelTutorialWarning()
    {
        noTutorialConfirmation.transform.position = new Vector3(100, 100, 100);
    }

    //Load the game with or without tutorial
    IEnumerator LoadGame(bool tutorial)
    {
        if (tutorial == false)
        {
            GameObject.Find("TutorialCheck").SetActive(false);
        }

        yield return new WaitForSeconds(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("wott_main");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

}
