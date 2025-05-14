using System.Collections.Generic;
using UnityEngine;

//When we press the journal button in the palm menu, open the journal ui in front of the user
public class OpenJournal : MonoBehaviour
{
    public GameObject Journal;
    public GameObject nextButton;
    public GameObject prevButton;

    public List<GameObject> imageBacks;
    public List<GameObject> images;

    public GameObject palmMenuTutorial;

    public void OpenJournalMenu()
    {
        //If we are not in our "grabbed butterfly" menu, open our journal, if we are, return
        if (!GameObject.FindWithTag("Fake"))
        {
            Vector3 vHeadPos = Camera.main.transform.position;
            Vector3 vGazeDir = Camera.main.transform.forward;
            Journal.transform.position = (vHeadPos + vGazeDir * .5f) + new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0;
            Journal.transform.eulerAngles = vRot;
        }
        else
        {
            return;
        }

    }

    //Put the journal somewhere hidden. Makes adding photos to it easier than having to re-enable/disable it everytime we take a photo.
    public void CloseJournalMenu()
    {
        if (GameObject.Find("TutorialCheck"))
        {
            palmMenuTutorial.SetActive(true);
        }

        Journal.transform.position = new Vector3(0, 0, 0);
    }
}
