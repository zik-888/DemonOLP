using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshSystem;

public class VertexPoint : MonoBehaviour
{
    private void Awake()
    {
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnMouseDown()
    {
        Debug.Log($"Vertex number: {name}, pos: {transform.position}");

        Debug.Log($"conformity: {CustomMeshPool.GetMesh(0).GetPositionVertexByNumber(int.Parse(name))}");
    }

    private void OnMouseEnter()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
    private void OnMouseExit()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}

public struct VertexPointStruct
{
    public int indexSurface;
    public int indexVertex;
    public Vector3 position;


}
