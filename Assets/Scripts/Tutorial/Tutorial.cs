using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

//Script that handles the sequencing of the tutorial. Needs heavy design and accessibility reformation
//After commenting, all "parts" of the tutorial could probably be condensed into a single method to avoid lots of code dupe
public class Tutorial : MonoBehaviour
{
    public bool isTalking;

    //This should probably be handled in a single Dictionary that accesses a text asset
    public List<string> locomotionLines;
    public Dictionary<int, float> locomotionPauses;
    public List<string> palmMenuLines;
    public Dictionary<int, float> palmMenuPauses;
    public List<string> cameraLines;
    public Dictionary<int, float> cameraPauses;
    public List<string> distanceLines;
    public Dictionary<int, float> distancePauses;
    public List<string> partLines;
    public Dictionary<int, float> partPauses;

    public TextToSpeech textToSpeech;

    public GameObject locoTutorial;
    public GameObject palmTutorial;
    public GameObject cameraTutorial;

    public GameObject distanceHandGrabInteractor;
    public GameObject digitalCamera;
    public GameObject locomotionInteractor;
    public GameObject palmMenu;

    public TextMeshProUGUI subtitles;

    public bool atDistanceGrabTutorial = false;

    void Start()
    {
        //If we are skipping the tutorial, make sure all our interactors are available for use, then return
        if (GameObject.Find("TutorialCheck") == null)
        {
            StartCoroutine(FadeFromBlack(3.0f));

            subtitles.text = "";
            digitalCamera.SetActive(true);
            palmMenu.SetActive(true);
            locomotionInteractor.SetActive(true);
            atDistanceGrabTutorial = true;
            distanceHandGrabInteractor.SetActive(true);
            return;
        }

        //Add all of our lines and additional pauses to our narrator's "script"
        locomotionLines.Add("Hello! Welcome to Wild XR Safari. My name is Matt, and I will be your guide.");
        locomotionLines.Add("First, we'll get you comfortable with moving around.");
        locomotionLines.Add("In order to move around, hold your right hand in front of you. A gesture tutorial will appear to the northeast of your hand. Make the exact same gesture as the tutorial in order to move. Note that some areas cannot be moved to, and will change your locomotion beam to red.");
        locomotionPauses = new Dictionary<int, float>()
        {
            { 0, 1.5f }  // 1.5 seconds pause after line 1 
        };

        palmMenuLines.Add("Next, we will learn about the Palm Menu.");
        palmMenuLines.Add("Follow the instructions on the panel in front of you to use the Palm Menu.");
        palmMenuLines.Add("To complete this tutorial, open the palm menu, explore its features, and then close the palm menu.");
        palmMenuPauses = new Dictionary<int, float>()
        {
            { 1, 2.0f }
        };

        cameraLines.Add("One of the most important tools an explorer needs is a camera! You can take this one.");
        cameraLines.Add("If you look down and slightly to the right, you will notice that your camera has been attached to your hip.");
        cameraLines.Add("To take a picture, simply grab the camera, aim the viewport, and then use your left hand to press the red button to take a picture.");
        cameraPauses = new Dictionary<int, float>()
        {
            { 0, 2.0f },
            { 1, 1.0f }
        };

        distanceLines.Add("Alright, you are almost done! I have one last thing to show you. Make sure your journal is closed.");
        distanceLines.Add("When you are ready, either move towards a butterfly or wait for one to come near you.");
        distanceLines.Add("When a butterfly is within 5 or so feet of you, reach your opened right hand out towards the butterfly. Then, pinch your index finger and thumb together.");
        distancePauses = new Dictionary<int, float>()
        {
            { 0, 2.0f }
        };

        partLines.Add("You can use both hands to pinch opposite ends of the butterfly, and then push your hands together or pull them apart to scale the butterfly.");
        partLines.Add("As you may have noticed, when you look at certain parts of the butterfly, they will highlight in a yellow color.");
        partLines.Add("In order to learn more about the highlighted part, pinch with your right hand while looking at the part.");
        partLines.Add("A progress meter will fill, and when it is full, release your pinch in order to select the highlighted part.");
        partLines.Add("If done correctly, the information panel above the butterfly will change to describe the part you selected.");
        partLines.Add("And that does it for the tutorial! If you would like to go through the tutorial again, exit to the main menu and restart the game.");
        partPauses = new Dictionary<int, float>()
        {
            { 0, 2.5f },
            { 1, 1.0f },
            { 3, 2.0f },
            { 4, 2.0f }
        };

        StartCoroutine(FadeFromBlack(3.0f));
        StartCoroutine(LocomotionTutorial());
    }

    //Fades scene in from black
    IEnumerator FadeFromBlack(float duration)
    {
        Color color = GameObject.Find("FadeFromBlack").GetComponent<MeshRenderer>().material.color;

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            // Calculate the new alpha based on the elapsed time
            float easedT = EaseInQuad(t / duration);
            float newAlpha = Mathf.Lerp(1.0f, 0.0f, easedT);
            color.a = newAlpha;

            // Apply the new alpha to the material
            color.a = newAlpha;
            GameObject.Find("FadeFromBlack").GetComponent<MeshRenderer>().material.color = color;

            // Wait for the next frame
            yield return null;
        }

        color.a = 0.0f;
        GameObject.Find("FadeFromBlack").GetComponent<MeshRenderer>().material.color = color;
        Destroy(GameObject.Find("FadeFromBlack"));

        
    }

    // Quadratic easing function (ease-in)
    float EaseInQuad(float t)
    {
        return t * t;
    }

    //First part of our tutorial, handling locomotion
    IEnumerator LocomotionTutorial()
    {
        for (int i = 0; i < locomotionLines.Count; i++)
        {
            subtitles.text = locomotionLines[i];

            //Call our method that calls TextToSpeech, and wait until it finishes 
            yield return SpeakAndWait(locomotionLines[i]);

            //If we have added a pause after a line, pause for the set duration
            if (locomotionPauses.ContainsKey(i))
            {
                float pauseDuration = locomotionPauses[i];
                yield return new WaitForSeconds(pauseDuration);
            }

            //Enable locomotion interactor after the second line of text is read
            if (i == 1)
            {
                locomotionInteractor.SetActive(true);
                locoTutorial.SetActive(true);
            }

        }

    }

    //Second part of the tutorial, handling the palm menu. Needs to be reworked to actually require interaction with the palm menu to progress.
    public IEnumerator PalmMenuTutorial()
    {
        //Ensure narrator is done praising the player for their locomotion skills
        yield return new WaitForSeconds(5);

        //Enable the palm menu tutorial as we start it
        palmTutorial.SetActive(true);

        for (int i = 0; i < palmMenuLines.Count; i++)
        {
            subtitles.text = palmMenuLines[i];

            //Call our method that calls TextToSpeech, and wait until it finishes 
            yield return SpeakAndWait(palmMenuLines[i]);

            //If we have added a pause after a line, pause for the set duration
            if (palmMenuPauses.ContainsKey(i))
            {
                float pauseDuration = palmMenuPauses[i];
                yield return new WaitForSeconds(pauseDuration);
            }

            //After the first line of text, display the palm menu tutorial video in front of the player, and enable the palm menu to be used
            if (i == 0)
            {
                Vector3 vHeadPos = Camera.main.transform.position;
                Vector3 vGazeDir = Camera.main.transform.forward;
                palmTutorial.transform.position = (vHeadPos + vGazeDir /** 3.0f*/) + new Vector3(0.0f, -.40f, 0.0f);
                Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0; vRot.x = 0;
                palmTutorial.transform.eulerAngles = vRot;
                palmTutorial.transform.position = GameObject.Find("UITargetSphere").transform.position;

                palmMenu.SetActive(true);
            }

            //Hide the subtitles after a short duration when the last line of speech for this section is said, again, should probably be removed in favor of requiring palm menu interaction to progress.
            if (i == 2)
            {
                yield return new WaitForSeconds(2);
                subtitles.text = "";
            }

        }
    }

    public void FirstClose()
    {
        palmTutorial.transform.position = new Vector3(0, 0, 0);
        palmTutorial.GetComponent<LockYAxisFollow>().enabled = false;
        palmTutorial.transform.position = new Vector3(0, 0, 0);
        palmTutorial.SetActive(false);
        StartCoroutine(CameraTutorial());
    }

    //Helper method to clean up our palm tutorial when we are finished with it, then starts the Camera tutorial
    //public void FinishPalmTutorial()
    //{
    //    palmTutorial.transform.position = new Vector3(0, 0, 0);
    //    palmTutorial.GetComponent<LockYAxisFollow>().enabled = false;
    //    palmTutorial.transform.position = new Vector3(0, 0, 0);
    //    palmTutorial.SetActive(false);
    //    StartCoroutine(CameraTutorial());
    //}

    //Third part of the tutorial, handling "digital camera" interaction
    public IEnumerator CameraTutorial()
    {
        //Set the position and rotation of "GivenCamera" UI
        Vector3 vHeadPos = Camera.main.transform.position;
        Vector3 vGazeDir = Camera.main.transform.forward;
        cameraTutorial.transform.position = (vHeadPos + vGazeDir /** 3.0f*/) + new Vector3(0.0f, -.40f, 0.0f);
        Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0; vRot.x = 0;
        cameraTutorial.transform.eulerAngles = vRot;
        cameraTutorial.transform.position = GameObject.Find("UITargetSphere").transform.position;

        for (int i = 0; i < cameraLines.Count; i++)
        {
            subtitles.text = cameraLines[i];

            //Call our method that calls TextToSpeech, and wait until it finishes 
            yield return SpeakAndWait(cameraLines[i]);

            //If we have added a pause after a line, pause for the set duration
            if (cameraPauses.ContainsKey(i))
            {
                float pauseDuration = cameraPauses[i];
                yield return new WaitForSeconds(pauseDuration);
            }

            //After the first line of dialogue, disable the "GivenCamera" ui and enable the real digital camera
            if (i == 0)
            {
                cameraTutorial.SetActive(false);
                digitalCamera.SetActive(true);
            }

        }
    }

    //Once the user takes a picture, let them know they were successful, then start the next part of the tutorial
    public async void TookFirstPicture()
    {
        subtitles.text = "Nice shot! As you can see, a preview of your photo will briefly appear in front of you. To look through your photos, use the Journal in the palm menu.";
        await textToSpeech.Speak("Nice shot! As you can see, a preview of your photo will briefly appear in front of you. To look through your photos, use the Journal in the palm menu.");
        StartCoroutine(DistanceHandGrabTutorial());
    }

    //Fourth part of tutorial, handles "distance grab" interactor and our "grabbable butterfly" menu
    IEnumerator DistanceHandGrabTutorial()
    {
        //Wait until camera dialogue is done
        yield return new WaitForSeconds(9);

        for (int i = 0; i < distanceLines.Count; i++)
        {
            subtitles.text = distanceLines[i];

            //Call our method that calls TextToSpeech, and wait until it finishes 
            yield return SpeakAndWait(distanceLines[i]);

            //If we have added a pause after a line, pause for the set duration
            if (distancePauses.ContainsKey(i))
            {
                float pauseDuration = distancePauses[i];
                yield return new WaitForSeconds(pauseDuration);
            }
            
            //After the second line of speech, enable the distance grab interactor 
            if (i == 1)
            {
                atDistanceGrabTutorial = true;
                distanceHandGrabInteractor.SetActive(true);
            }

        }
    }

    //Once the user grabs a butterfly, let them know they were successful, and start the next part of the tutorial
    public async void FirstGrab()
    {
        subtitles.text = "Boom! As you can see, you now have a close up visual of the butterfly you reached for, as well as its description above it.";
        await textToSpeech.Speak("Boom! As you can see, you now have a close up visual of the butterfly you reached for, as well as its description above it.");
        StartCoroutine(PartExamineTutorial());
    }

    //Last part of the tutorial, handles "PinchGrabSelector" interaction to learn more about butterfly anatomy
    IEnumerator PartExamineTutorial()
    {
        //Wait until distance grab dialogue is over
        yield return new WaitForSeconds(8);

        for (int i = 0; i < partLines.Count; i++)
        {
            subtitles.text = partLines[i];

            //Call our method that calls TextToSpeech, and wait until it finishes 
            yield return SpeakAndWait(partLines[i]);

            //If we have added a pause after a line, pause for the set duration
            if (partPauses.ContainsKey(i))
            {
                float pauseDuration = partPauses[i];
                yield return new WaitForSeconds(pauseDuration);
            }

        }

        //Clear our subtitles after the last line of tutorial dialogue, then disable this script.
        subtitles.text = "";
        this.gameObject.GetComponent<Tutorial>().enabled = false;
    }

    //Calls TextToSpeech.cs and sends in the line of dialogue, will wait until dialogue is finished before returning to the method
    IEnumerator SpeakAndWait(string line)
    {
        Task speakTask = textToSpeech.Speak(line);

        yield return new WaitUntil(() => speakTask.IsCompleted);

        while (textToSpeech.audioSource.isPlaying)
        {
            yield return null;  //Wait for the next frame
        }
    }

    //Helper method for my lazy tutorial instantiations that I created to avoid writing an assembly reference. Needs to be fixed ASAP.
    public async void DidSomething(string line)
    {
        subtitles.text = line;
        await textToSpeech.Speak(line);
    }

    //Some of our menus (pause, quit, journal) disable the distance grab interactor when they are enabled, then re-enbable it when they are closed
    //This is my way of making sure it is always disabled until we get to the proper part of the tutorial. Can definitely be made better
    void Update()
    {
        if (atDistanceGrabTutorial == false)
        {
            if (GameObject.Find("DistanceHandGrabInteractor") != null)
            {
                GameObject.Find("DistanceHandGrabInteractor").SetActive(false);
            }
        }
    }
}
