using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNMeshSystem : MonoBehaviour
{
    public NMesh nMesh;

    // Start is called before the first frame update
    void Start()
    {
        nMesh = new NMesh(new Vector3[] { new Vector3(1, 2, 3), new Vector3(4, 5, 6), new Vector3(7, 8, 9), new Vector3(10, 11, 12) }, 
                          new int[] { 0, 1, 2, 2, 1, 3 });


    }
}
