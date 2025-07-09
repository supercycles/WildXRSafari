using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script used for returning to the main menu from in game
public class ExitToMenu : MonoBehaviour
{
    public GameObject exitMenu;
    public GameObject fake;
    public GameObject palmMenuTutorial;

    //Give the player a warning if they try to quit the game from the palm menu
    public void ExitWarning()
    {
        Vector3 vHeadPos = Camera.main.transform.position;
        Vector3 vGazeDir = Camera.main.transform.forward;
        Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0; vRot.x = 0;
        exitMenu.transform.eulerAngles = vRot;
        exitMenu.transform.position = GameObject.Find("UITargetSphere").transform.position;
        exitMenu.transform.position = new Vector3(exitMenu.transform.position.x, exitMenu.transform.position.y - .1f, exitMenu.transform.position.z);

        //If we are in the "grabbed butterfly menu", disable the butterfly until we resume from the quit menu
        fake = GameObject.FindWithTag("Fake");
        if (fake != null) { fake.SetActive(false); }
    }

    //If we confirm that we want to exit after the warning, load the main menu
    public void ExitToMainMenu()
    {
        StartCoroutine(LoadMenu());
    }

    //If we decline to exit after the warning, close the menu and re-enable grabbable butterfly menu (if we were there in the first place)
    public void CancelExit()
    {
        exitMenu.transform.position = new Vector3(0, 0, 0);
        if (fake != null) { fake.SetActive(true); }
        if (GameObject.Find("TutorialCheck"))
        {
            palmMenuTutorial.SetActive(true);
        }
    }

    //Closes and unloads current scene while loading and opening the main menu
    IEnumerator LoadMenu()
    {
        Scene currScene = SceneManager.GetActiveScene();

        if(GameObject.Find("TutorialCheck") != null)
        {
            Destroy(GameObject.Find("TutorialCheck"));
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(currScene);
    }
}
