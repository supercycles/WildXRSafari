using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMeshesOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshCombiner meshCombiner = gameObject.AddComponent<MeshCombiner>();

        meshCombiner.CreateMultiMaterialMesh = true;
        meshCombiner.DestroyCombinedChildren = true;

        meshCombiner.CombineMeshes(false);
    }

}
