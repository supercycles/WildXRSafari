using UnityEngine;

public class KillFake : MonoBehaviour
{
    //When we exit out of our "grabbed butterfly" menu, delete the fake that we were able to rotate/scale/inspect
    public void KillFakeButterfly()
    {
        Destroy(GameObject.FindWithTag("Fake"));

        GameObject.Find("PartInfo").transform.position = new Vector3(0, 0, 0);
    }

}
