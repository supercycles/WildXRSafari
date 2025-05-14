using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This script is used once we have grabbed a butterfly, it raycasts from the user's gaze onto the butterfly and highlights the part that the ray hits
//NOTE: Only needs to be turned on when we are in "grabbable menu"
public class HeadRay : MonoBehaviour
{
    public Camera playerCamera;
    public Transform laserOrigin;
    public float sightRange = 20f;
    public Image hitPoint;
    public Image hitPointLoad;
    public Image noHitPoint;

    public Material highlight;

    LineRenderer laserLine;

    private GameObject lastHitObject = null;
    public bool pinched = false;

    public TextMeshPro partText;
    public string textDisplayKey;

    void Awake()
    {
        laserLine = GetComponent<LineRenderer>();
        noHitPoint.enabled = false;
        hitPoint.enabled = false;
        hitPointLoad.enabled = false;
    }

    void Update()
    {
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * sightRange));

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, sightRange))
        {
            //Check what object we hit
            GameObject hitObject = hit.collider.gameObject;
            //If the object we have hit is not the same as the object we hit in the last frame, unhighlight the previously hit object
            if (hitObject != lastHitObject)
            {
                if (lastHitObject != null)
                {
                    try
                    {
                        UnHighlightParts(lastHitObject);
                    }
                    catch { }
                }

                //The object we have hit will be checked in the next frame to determine if we are still hitting it
                lastHitObject = hitObject;
            }

            //Do we hit a part on our grabbed butterfly? (can probably be done in a way that doesn't require names)
            //NOTE: the butterfly itself has a collider that we use for rotating/scaling it, it is important that we set the parent object with this collider to "Ignore Raycast"
            if (hitObject.name.Contains("bfs"))
            {
                //Enable our "cursor" at the point our ray is hitting the butterfly
                hitPoint.enabled = true;
                hitPoint.transform.position = hit.point + hit.normal * 0.001f;
                hitPoint.transform.LookAt(hitPoint.transform.position + hit.normal);

                if (hitPointLoad.enabled == true)
                {
                    hitPointLoad.transform.position = hit.point + hit.normal * 0.001f;
                    hitPointLoad.transform.LookAt(hitPoint.transform.position + hit.normal);
                }

                //Highlight whatever we hit
                HighlightParts(hitObject);

                //"pinched" means if we pinched until the hit point load meter was filled, and released when it was full (Check CustomIndexPinchReleaseSelector.cs)
                if (pinched)
                {
                    pinched = false;
                    hitPointLoad.fillAmount = 0;

                    //Set the key for the text we want to display of the selected part
                    GameObject.Find("EventSystem").GetComponent<InfoTextManager>().AssignInfoText(textDisplayKey);
                }
            }
            //If we aren't hitting the butterfly, disable the cursor
            else
            {
                hitPoint.enabled = false;
                hitPointLoad.fillAmount = 0;
                hitPointLoad.enabled = false;
            }
        }
        //If we aren't hitting anything, disable the cursor and also unhighlight the last object we were looking at, if it is not null
        else
        {
            if (lastHitObject != null)
            {
                try
                {
                    UnHighlightParts(lastHitObject);
                    textDisplayKey = "";
                } 
                catch { }
                lastHitObject = null;
            }

            hitPoint.enabled = false;
            hitPointLoad.fillAmount = 0;
            hitPointLoad.enabled = false;
        }
    }

    //Helper method used to seelct which parts we need to highlight based on the part we hit with our raycast
    public void HighlightParts(GameObject part)
    {
        //Get the parent of the part, as we will be looping through its children later
        GameObject partParent = part.transform.parent.gameObject;
        //Get the name of the part(again, could probably be done better than through name handling, maybe through tags?)
        string fullPartName = part.name;
        string[] subStrings;
        string subPartName = "";

        //Get the actual name of the part. example format: bfs_Hindwing_Right
        //We want the middle string in between the two underscores
        subStrings = fullPartName.Split('_');
        subPartName = subStrings[1];

        //The purpose for this loop is to find all parts that match the subPartName we just grabbed from our part
        //Our loop skips over the first two children of our parent (the butterfly), because those have been reserved for the necessary "grab interactable" objects
        //It also stops before hitting the last two children of the butterfly, which are the colliders for our legs.
        //Probably inefficient
        for (int i = 2; i < partParent.transform.childCount - 2; i++)
        {
            GameObject temp;
            string[] subStringsTemp;
            string subPartNameTemp = "";

            //Check each child of our butterfly and once again look for the string that names the part
            temp = partParent.transform.GetChild(i).gameObject;

            subStringsTemp = temp.name.Split('_');
            subPartNameTemp = subStringsTemp[1];

            //This was conceptualized as a massive if else chain, so I'll take this for now
            //If the current child matches the name of our hit object, highlight it
            //Also temporarily set the key for what text we want to display IF this part ends up being selected
            if (subPartNameTemp == "Leg")
            {
                if (subPartName == "Legs")
                {
                    HighlightPart(temp);
                    textDisplayKey = "Legs";
                }
            }
            else if (subPartNameTemp == subPartName)
            {
                HighlightPart(temp);
                textDisplayKey = subPartName;
            }
            else if (subPartName == "Head")
            {
                if (subPartNameTemp == "Antenne" || subPartNameTemp == "Eye" || subPartNameTemp == "Proboscis")
                {
                    HighlightPart(temp);
                    textDisplayKey = "Head";
                }
            }
  
        }
    }

    //Helper method to highlight the proper children of the butterfly
    public void HighlightPart(GameObject part)
    {
        //Our "highlight" is just another material that is yellow and partially transparent layered over our original material
        Material[] materials = new Material[2];
        materials[0] = part.GetComponent<MeshRenderer>().material;
        materials[1] = highlight;

        part.GetComponent<MeshRenderer>().materials = materials;
    }

    //When we are no longer looking at a part of the butterfly, unhighlight the part and all its associates
    public void UnHighlightParts(GameObject parts)
    {
        GameObject partParent = parts.transform.parent.gameObject;

        for (int i = 2; i < partParent.transform.childCount - 2; i++)
        {
            Material[] materials = new Material[1];
            materials[0] = partParent.transform.GetChild(i).GetComponent<MeshRenderer>().materials[0];

            partParent.transform.GetChild(i).GetComponent<MeshRenderer>().materials = materials;
        }
    }

}
