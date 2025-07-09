using UnityEngine;

//Pauses the game when we select the "Pause Game" button in the palm menu
public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject pauseBack;
    public GameObject fake;
    public GameObject palmMenuTutorial;
    public bool paused = false;

    //I noticed that Jacob was trying to use the same button in the palm menu he used to pause to also resume the game, maybe move to that functionality? Maybe allow both the menu and the palm button to resume?
    public void Pause()
    {
        if (!paused)
        {
            pauseMenu.SetActive(true);

            //Set pos and rot of our pause menu
            Vector3 vHeadPos = Camera.main.transform.position;
            Vector3 vGazeDir = Camera.main.transform.forward;
            pauseMenu.transform.position = (vHeadPos + vGazeDir /** 3.0f*/) + new Vector3(0.0f, -.40f, 0.0f);
            Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0; vRot.x = 0;
            pauseMenu.transform.eulerAngles = vRot;
            pauseButton.transform.position = GameObject.Find("UITargetSphere").transform.position; 
            pauseBack.transform.position = GameObject.Find("UITargetSphere").transform.position; 

            //If we have our "grabbable butterfly" menu open, close it until we resume
            fake = GameObject.FindWithTag("Fake");
            if (fake != null) { fake.SetActive(false); }
            paused = true;

            //Other pause functionalities are handled through events in the Inspector

            //Jacob warned of unpredictable behavior, check other methods to pause 
            Time.timeScale = 0.0f;
        }
        else
        {
            return;
        }
    }
    public void Resume()
    {
        //If we had our "grabbable butterfly" menu open, reopen it when we resume
        if (fake != null)
        {
            fake.SetActive(true);
        }

        if (GameObject.Find("TutorialCheck"))
        {
            palmMenuTutorial.SetActive(true);
        }

        //Close our pause menu
        pauseMenu.SetActive(false);

        //Start time back up
        Time.timeScale = 1.0f;
        paused = false;
    }
}
