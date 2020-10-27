using MeshSystem;
using RosSharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using thelab.mvc;
using UnityEngine;

public class PointToPoint : Element<DemonOLPApplication>, ITrajectory
{
    public GameObject parentObject;

    protected List<ReferencePoint> referencePoints = new List<ReferencePoint>();

    maxCamera mxc;
    public float basePointSize = 0.1f;
    public float pointSize = 0.1f;

    private void Start()
    {
        mxc = Camera.main.GetComponent<maxCamera>();
        app.model.Trajectories.Add(this);
    }

    private void OnDestroy()
    {
        app.model.Trajectories.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        var camDistance = mxc.DesiredDistance;

        basePointSize = camDistance / 50;
        pointSize = camDistance / 50;

        foreach (var a in referencePoints)
        {
            a.transform.localScale = Vector3.one * basePointSize;
        }

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
        referencePoints.Last().transform.position = pos;
    }

    public Vector3[] GetPositions()
    {
        var pos = from p in referencePoints
                  select p.transform.position;

        return pos.ToArray();
    }

    public Quaternion[] GetRotations()
    {
        var rot = from n in referencePoints
                  let v = TransformExtensions.Unity2Ros(CustomMeshPool.GetMesh(0).Triangles[n.triangleIndex].Normal)
                  select new Quaternion 
                  {
                      x = v.x,
                      y = v.y,
                      z = v.z,
                      w = 0
                  };

        

        return rot.ToArray();
    }
}
