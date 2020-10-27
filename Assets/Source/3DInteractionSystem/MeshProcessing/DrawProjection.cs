using MeshSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using thelab.mvc;
using UnityEngine;

public class DrawProjection : Element<DemonOLPApplication>
{
    public GameObject parentObject;
    protected LineRenderer lineRenderer;

    protected List<ReferencePoint> referencePoints = new List<ReferencePoint>();
    protected List<GameObject> additionalPoints = new List<GameObject>();


    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;

        

    }


    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)
            || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)
            || hit.triangleIndex == -1)
            return;

        AddPoint(hit.point);

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Debug.Log($"Triangle index: {hit.triangleIndex}, " +
                  $"Plane index: {CustomMeshPool.GetMesh(0).Triangles[hit.triangleIndex].IndexPlane}");

        referencePoints.Last().triangleIndex = hit.triangleIndex;
    }

    protected void AddPoint(Vector3 pos)
    {
        referencePoints.Add(Instantiate(app.model.refPoint, parentObject.transform).GetComponent<ReferencePoint>());
        referencePoints.Last().useMethod = UseMethod;
        referencePoints.Last().transform.position = pos;

    }

    protected void UseMethod()
    {
        //Physics.Raycast(Camera.main.ScreenPointToRay(, out RaycastHit hit);
    }

}
