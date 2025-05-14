using DG.Tweening;
using System.Collections;
using UnityEngine;

/*When we "Distance Grab" a butterfly, it instantiates a prefab of that butterfly that posseses this script.
We move the butterfly with this script to the proper position in front of the player, then replace it with a "grabbable" version that allows scaling/rotating.
This second replacement is necessary because "GrabFreeTransformer", which locks the position and minmax scale of the grabbable enables on Awake and can't be disabled,
so we want the grabbable to be instantiated where we will lock its position*/
public class DistanceGrabReplacement : MonoBehaviour
{
    public GameObject grabbable;

    public GameObject blue;
    public GameObject leopard;
    public GameObject mala;
    public GameObject paper;
    public GameObject post;
    public GameObject tiger;

    // Start is called before the first frame update
    void Awake()
    {
        //Disable locomotion when we have grabbed an object
        GameObject.Find("LocomotionHandInteractorGroup").SetActive(false);

        //Set the target postion to move the initial replacement
        Vector3 vHeadPos = Camera.main.transform.position;
        Vector3 vGazeDir = Camera.main.transform.forward;
        Vector3 targetPos = vHeadPos + vGazeDir * .6f;
        targetPos.y = vHeadPos.y -.05f;

        StartCoroutine(WaitForMove(targetPos));
    }

    //Move the initial replacement
    IEnumerator WaitForMove(Vector3 tPos)
    {
        this.transform.DOMove(GameObject.Find("UITargetSphere").transform.position, 0.2f);

        yield return new WaitForSeconds(.2f);

        ReplaceWithGrabbable();
    }    

    //Once the initial replacement has reached its destination, we instantiate the grabbable in its place and delete the initial
    void ReplaceWithGrabbable()
    {
        string grabbableName;

        //Probably can be optimized
        grabbableName = this.gameObject.name;
        if (grabbableName == ("BM Move(Clone)")) { grabbable = blue; GameObject.Find("EventSystem").GetComponent<InfoTextManager>().AssignInfoText("BLUE"); }
        else if (grabbableName == ("LL Move(Clone)")) { grabbable = leopard; GameObject.Find("EventSystem").GetComponent<InfoTextManager>().AssignInfoText("LEOPARD"); }
        else if (grabbableName == ("MA Move(Clone)")) { grabbable = mala; GameObject.Find("EventSystem").GetComponent<InfoTextManager>().AssignInfoText("MALA"); }
        else if (grabbableName == ("PK Move(Clone)")) { grabbable = paper; GameObject.Find("EventSystem").GetComponent<InfoTextManager>().AssignInfoText("PAPER"); }
        else if (grabbableName == ("PM Move(Clone)")) { grabbable = post; GameObject.Find("EventSystem").GetComponent<InfoTextManager>().AssignInfoText("POST"); }
        else if (grabbableName == ("TL Move(Clone)")) { grabbable = tiger; GameObject.Find("EventSystem").GetComponent<InfoTextManager>().AssignInfoText("TIGER"); }

        Instantiate(grabbable, this.transform.position, this.transform.rotation);

        //Set pos+rot of UI that appears when we grab a butterfly
        Vector3 vRot = Camera.main.transform.eulerAngles; vRot.z = 0; vRot.x = 0;
        GameObject.Find("PartInfo").transform.position = GameObject.Find("UITargetSphere").transform.position;
        GameObject.Find("PartInfo").transform.eulerAngles = vRot;
        
        //Enable our headray script that highlights/selects parts of the grabbable butterfly
        Camera.main.gameObject.GetComponent<HeadRay>().enabled = true;

        //Destroy the initial replacement
        Destroy(this.gameObject);
    }

}
