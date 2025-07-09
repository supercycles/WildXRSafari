using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;

//Script used to take pictures with our "digital camera"
//Some code provided by ChatGPT
public class ScreenCapture : MonoBehaviour
{
    public RenderTexture overviewTexture;
    GameObject OVcamera;
    public string path = "";
    public AudioSource click;
    public GameObject polaroid;
    public GameObject polaroidBack;
    public Camera mainCam;
    public GameObject body;
    GameObject picCanvas;

    public RectTransform polaroidRect;
    public RectTransform polaroidBackRect;
    public float topPosY;
    public float bottomPosY;
    public float tweenDuration;
    public GameObject player;

    public float testY;
    public float testX;

    public bool firstPic = false;

    void Start()
    {
        Debug.unityLogger.logEnabled = true; //Should probably have this somewhere else. For some reason, the Debug log is disabled without this, I can't find where it is being disabled though.
        OVcamera = GameObject.FindGameObjectWithTag("OverviewCamera");
        picCanvas = GameObject.Find("PolaroidCanvas"); 
    }

    public void TakePicture()
    {
        //Tutorial check to see if this is the first picture we are taking. If we are in the tutorial and it is, advance the tutorial
        //Otherwise, do nothing
        if (firstPic == false && GameObject.Find("TutorialCheck") == true)
        {
            firstPic = true;
            GameObject.Find("EventSystem").GetComponent<Tutorial>().TookFirstPicture();
        }

        //TakeScreenShot() is a messy coroutine that needs a rewrite. StopCoroutine should not be necessary here
        StopCoroutine(TakeScreenShot());
        StartCoroutine(TakeScreenShot());
    }

    //Return file name for captured photo. Currently unused, may be used later if we want to save the photos
    string fileName(int width, int height)
    {
        return string.Format("screen_{0}x{1}_{2}.png",
                              width, height,
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public IEnumerator TakeScreenShot()
    {
        Debug.Log("Coroutine started");
        yield return new WaitForEndOfFrame();

        //Find "Digital Camera" camera and assign render texture with capture
        OVcamera = GameObject.FindGameObjectWithTag("OverviewCamera");
        Camera camOV = OVcamera.GetComponent<Camera>();

        //Set render texture to target texture "imageOverview", then set render texture back to active
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = camOV.targetTexture;
        camOV.Render();
        Texture2D imageOverview = new Texture2D(camOV.targetTexture.width, camOV.targetTexture.height, TextureFormat.RGB24, false);
        imageOverview.ReadPixels(new Rect(0, 0, camOV.targetTexture.width, camOV.targetTexture.height), 0, 0);
        imageOverview.Apply();
        RenderTexture.active = currentRT;

        //Brighten Texture
        Color[] pixels = imageOverview.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            var p = pixels[i];
            p.r = AdjustChannel(p.r, .15f, 1, 1);
            p.g = AdjustChannel(p.g, .15f, 1, 1);
            p.b = AdjustChannel(p.b, .15f, 1, 1);
            pixels[i] = p;
        }

        imageOverview.SetPixels(pixels);
        imageOverview.Apply();

        //Apply texture to our "photo" object, set up full "polaroid" with background
        GameObject takenPictureBack = Instantiate(polaroidBack, picCanvas.transform.position, Quaternion.identity, picCanvas.transform);
        GameObject takenPicture = Instantiate(polaroid, new Vector3(picCanvas.transform.position.x, picCanvas.transform.position.y, picCanvas.transform.position.z), Quaternion.identity, picCanvas.transform);
        takenPicture.GetComponentInChildren<UnityEngine.UI.Image>().sprite = Sprite.Create(imageOverview, new Rect(0, 0, imageOverview.width, imageOverview.height), new Vector2(0, 0));
        takenPicture.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -2000);
        takenPictureBack.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -2000);

        //Get the RectTransform of our "polaroid" prefabs in order to tween them
        polaroidRect = takenPicture.GetComponent<RectTransform>();
        polaroidBackRect = takenPictureBack.GetComponent<RectTransform>();

        //Play picture taken audio
        click.Play();

        //Tween the "polaroid" onto the screen briefly, then tween it out. This section of code is particularly messy and needs revision
        polaroidRect.DOAnchorPosY(topPosY + 35, tweenDuration);
        polaroidBackRect.DOAnchorPosY(topPosY, tweenDuration);
        takenPicture.transform.rotation = Quaternion.LookRotation(takenPicture.transform.position - mainCam.transform.position);
        takenPictureBack.transform.LookAt(mainCam.transform.position);
        polaroidRect.DOLocalRotate(new Vector3(testX, takenPicture.transform.rotation.y, takenPicture.transform.rotation.z), .001f);
        polaroidBackRect.DOLocalRotate(new Vector3(testX, takenPicture.transform.rotation.y, takenPicture.transform.rotation.z), .001f);
        yield return new WaitForSeconds(2);
        polaroidRect.DOAnchorPosY(bottomPosY, tweenDuration);
        polaroidBackRect.DOAnchorPosY(bottomPosY, tweenDuration);
        yield return new WaitForSeconds(.5f);

        //Send it to the Journal
        GameObject.Find("Journal").GetComponent<BuildJournal>().AddPolaroid(takenPicture, takenPictureBack);

        //Again, if we want to eventually save the photos to the device/even send them over network, this might be used
        //// Encode texture into PNG
        //byte[] bytes = imageOverview.EncodeToPNG();

        //// save in memory
        //string filename = fileName(Convert.ToInt32(imageOverview.width), Convert.ToInt32(imageOverview.height));
        //path = Application.persistentDataPath + "/Snapshots/" + filename;
        //System.IO.File.WriteAllBytes(path, bytes);

    }

    //Helper method to adjust the RGB channels of our photo, I needed this to increase the brightness of the photos
    public static float AdjustChannel(float colour,
           float brightness, float contrast, float gamma)
    {
        return Mathf.Pow(colour, gamma) * contrast + brightness;
    }
}