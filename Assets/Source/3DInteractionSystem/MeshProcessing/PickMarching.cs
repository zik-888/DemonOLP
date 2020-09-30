using MeshSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System;
using thelab.mvc;
using Random = UnityEngine.Random;
//using System.Numerics;

public class PickMarching : Element<DemonOLPApplication>
{
    public int idReferencePoint = 0;

    protected ReferencePoint[] referencePoints = new ReferencePoint[3];
    protected List<GameObject> additionalPoints = new List<GameObject>();

    protected LineRenderer lineRenderer;

    public GameObject parentObject;
    public GameObject planeDBG;
    public float basePointSize = 0.02f;
    public float pointSize = 0.02f;

    maxCamera mxc;

    // Start is called before the first frame update
    void Start()
    {
        mxc = Camera.main.GetComponent<maxCamera>();

        for (int i = 0; referencePoints.Length > i; i++)
        {
            referencePoints[i] = Instantiate(app.model.refPoint, parentObject.transform).GetComponent<ReferencePoint>();
            referencePoints[i].useMethod = UseMethod;
            referencePoints[i].transform.localScale = Vector3.one * basePointSize;
        }

        referencePoints[1].GetComponent<MeshRenderer>().material.color = Color.green;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        var camDistance = mxc.DesiredDistance;

        basePointSize = camDistance / 100;
        pointSize = camDistance / 100;

        foreach(var a in referencePoints)
        {
            a.transform.localScale = Vector3.one * basePointSize;
        }

        foreach(var a in additionalPoints)
        {
            a.transform.localScale = Vector3.one * pointSize;
        }

        lineRenderer.startWidth = camDistance / 200;
        lineRenderer.endWidth = camDistance / 200;


        if (!Input.GetMouseButtonDown(0)
            || !Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) 
            || hit.triangleIndex == -1)
            return;

        


        referencePoints[idReferencePoint].transform.position = hit.point;

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Debug.Log($"Triangle index: {hit.triangleIndex}, " +
                  $"Plane index: {CustomMeshPool.GetMesh(0).Triangles[hit.triangleIndex].IndexPlane}");

        referencePoints[idReferencePoint].triangleIndex = hit.triangleIndex;

        TogleSphere();
        
        #region Test2
        //Mesh mesh = meshCollider.sharedMesh;
        //Vector3[] vertices = mesh.vertices;
        //int[] triangles = mesh.triangles;


        //Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        //Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        //Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        //Color color = new Color(Random.value, Random.value, Random.value, 1);

        //Transform hitTransform = hit.collider.transform;
        //p0 = hitTransform.TransformPoint(p0);
        //p1 = hitTransform.TransformPoint(p1);
        //p2 = hitTransform.TransformPoint(p2);
        //Debug.DrawLine(p0, p1, color, 300f);
        //Debug.DrawLine(p1, p2, color, 300f);
        //Debug.DrawLine(p2, p0, color, 300f);
        #endregion
    }

    protected void TogleSphere()
    {
        //if (idReferencePoint == referencePoints.Length - 1)
        //{
        //    idReferencePoint = 0;
        //    UseMethod();
        //}
        //else
        //    idReferencePoint++;

        switch (idReferencePoint)
        {
            case 0:
                idReferencePoint = 2;
                break;
            case 1:
                UseMethod();
                idReferencePoint = 0;
                break;
            case 2:
                idReferencePoint = 1;
                break;
        }

    }

    private void UseMethod()
    {
        /// удаляем точки
        /// 

        foreach (var a in additionalPoints)
        {
            Destroy(a);
        }

        additionalPoints.Clear();

        /// используем метод
        /// 

        var marchingPoints = MarchingMethod(0, referencePoints, ref app.model.planeDNG);

        /// рисуем линию
        /// 

        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.007f;
        lineRenderer.endWidth = 0.007f;
        lineRenderer.positionCount = marchingPoints.Count();
        lineRenderer.SetPositions(marchingPoints.ToArray());


        /// рисуем точки 
        /// 

        for (int i = 1; marchingPoints.Count() - 1 > i; i++)
        {
            additionalPoints.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            additionalPoints.Last().transform.parent = parentObject.transform;
            additionalPoints.Last().transform.position = marchingPoints.ToArray()[i];
            additionalPoints.Last().transform.localScale = Vector3.one * pointSize;
            additionalPoints.Last().GetComponent<MeshRenderer>().material.color = Color.cyan;
            additionalPoints.Last().name = i.ToString();
        }
    }


    /// <summary>
    /// Marching method с определением плоскостей
    /// </summary>
    /// <param name="meshNum"> Mesh id </param>
    /// <param name="pointArray"> 3 points </param>
    public static Vector3[] MarchingMethod(int meshNum, ReferencePoint[] referencePoints, ref GameObject planeDBG)
    {
        Vector3[] pointArray = referencePoints.Select(p => p.transform.position).ToArray();

        Plane marchPlane = new Plane(pointArray[0], pointArray[1], pointArray[2]);

        Destroy(planeDBG);
        planeDBG = Instantiate(planeDBG, referencePoints[1].transform);
        planeDBG.transform.rotation = Quaternion.LookRotation(marchPlane.normal) * Quaternion.FromToRotation(Vector3.forward, Vector3.up);

        Vector3 midpoint = (pointArray[0] + pointArray[2]) / 2;

        var marchingPoints = from T in CustomMeshPool.GetMesh(meshNum).Triangles
                             from E in T.Edges
                             let e = Vector3.Dot(marchPlane.normal, E.v0.Position - E.v1.Position)
                             where e != 0
                             let o = new
                             {
                                 position = E.v1.Position + (E.v0.Position - E.v1.Position)
                                            * Vector3.Dot(marchPlane.normal, pointArray[0] - E.v1.Position) / e,
                                 edge = E
                             }
                             where Vector3.Dot(E.v1.Position - o.position, E.v0.Position - o.position) <= 0
                             select o;

        ///

        LinkedList<MarchingPoint> chain = new LinkedList<MarchingPoint>();

        try
        {
            var groupToEdgeMP = marchingPoints.GroupBy(p => p.edge, new EdgeComparer())
                                          .Select(p => new MarchingPoint(p.First().position, p.Select(g => g.edge).ToArray()))
                                          .ToList();

            

            print(groupToEdgeMP[0] == groupToEdgeMP[0]);

            groupToEdgeMP.Where(g => g.edges.Any(e => e.TriangleIndex == referencePoints[1].triangleIndex)).First()
                                    .SetBrother(chain, groupToEdgeMP, 
                                                new Vector3Int(referencePoints[0].triangleIndex, referencePoints[1].triangleIndex, referencePoints[2].triangleIndex),
                                                pointArray[0], pointArray[2]);

        }
        catch
        {
            foreach(var a in pointArray)
            {
                chain.AddFirst(new MarchingPoint(a));
            }
        }




        #region Test

        //Debug.DrawLine(pointArray[0], pointArray[1], Color.black);
        //Debug.DrawLine(pointArray[1], pointArray[2], Color.black);

        //Debug.DrawLine(pointArray[0], pointArray[2], Color.blue);
        //Debug.DrawLine(midpoint, midpoint + marchPlane.normal, Color.blue);

        //Debug.DrawLine(pointArray[0], pointArray[2], Color.green);
        //Debug.DrawLine(midpoint, midpoint + ortoMarchPlane.normal, Color.green);

        #endregion

        return chain.Select(c => c.position).ToArray();
    }

    /// <summary>
    /// Марчинг метод без определения плоскостей
    /// </summary>
    /// <param name="meshNum"> Mesh id </param>
    /// <param name="pointArray"> 3 points </param>
    public static Vector3[] MarchingMethod(int meshNum, Vector3[] pointArray)
    {
        Plane marchPlane = new Plane(pointArray[0], pointArray[1], pointArray[2]);

        Vector3 midpoint = (pointArray[0] + pointArray[2]) / 2;


        var marchingPoints = from T in CustomMeshPool.GetMesh(meshNum).Triangles
                             from E in T.Edges.AsParallel()
                             let e = Vector3.Dot(marchPlane.normal, E.v0.Position - E.v1.Position)
                             where e != 0
                             let o = E.v1.Position + (E.v0.Position - E.v1.Position)
                             * Vector3.Dot(marchPlane.normal, pointArray[0] - E.v1.Position) / e
                             where Vector3.Dot(E.v1.Position - o, E.v0.Position - o) <= 0
                             select o;

        Plane ortoMarchPlane = new Plane(Vector3.Cross(pointArray[2] - pointArray[0], marchPlane.normal),
                                         pointArray[0]);

        marchingPoints = from P in marchingPoints
                         where ortoMarchPlane.GetSide(P)
                         select P;

        marchingPoints = marchingPoints.Concat(pointArray)
                         .OrderBy(p => Vector3.SignedAngle(pointArray[1] - midpoint, p - midpoint, marchPlane.normal))
                         .Distinct(new Vector3Comparer());

        #region Test

        //Debug.DrawLine(pointArray[0], pointArray[1], Color.black);
        //Debug.DrawLine(pointArray[1], pointArray[2], Color.black);

        //Debug.DrawLine(pointArray[0], pointArray[2], Color.blue);
        //Debug.DrawLine(midpoint, midpoint + marchPlane.normal, Color.blue);

        //Debug.DrawLine(pointArray[0], pointArray[2], Color.green);
        //Debug.DrawLine(midpoint, midpoint + ortoMarchPlane.normal, Color.green);

        #endregion

        return marchingPoints.ToArray();
    }


    private void OnDestroy()
    {
        foreach(var a in additionalPoints)
        {
            Destroy(a);
        }

        foreach(var a in referencePoints)
        {
            Destroy(a);
        }
    }

}





public class MarchingPoint 
{ 
    public Vector3 position;
    public Edge[] edges = new Edge[2];
    public int[] triangleID = new int[2];
    public LinkedListNode<MarchingPoint> SelfNode;

    protected Color color = new Color(Random.value, Random.value, Random.value, 1);

    public MarchingPoint(Vector3 pos, Edge[] edges)
    {
        position = pos;
        this.edges[0] = edges[0];
        this.edges[1] = edges[1];

        triangleID[0] = edges[0].TriangleIndex;
        triangleID[1] = edges[1].TriangleIndex;
    }
    public MarchingPoint(Vector3 pos)
    {
        position = pos;
    }

    public void SetBrother(in LinkedList<MarchingPoint> chain, in List<MarchingPoint> groupPoint, Vector3Int triangleHL, Vector3 sP, Vector3 fP)
    {
        groupPoint.Remove(this);

        var brotherNode = from g in groupPoint
                          from t1 in g.triangleID
                          from t2 in triangleID
                          where t1 == t2
                          select g;

        if (chain.Count == 0)
        {

            Debug.Log($"0 This: {this}, first: {brotherNode.First()}, last: {brotherNode.Last()}");

            SelfNode = new LinkedListNode<MarchingPoint>(this);
            chain.AddFirst(SelfNode);

            if (!Between(triangleHL.x, brotherNode.Last(), this)
             && !Between(triangleHL.z, brotherNode.Last(), this))
            {
                chain.AddFirst(brotherNode.Last().CreateNode());
                brotherNode.Last().SetBrother(chain, groupPoint, triangleHL, sP, fP);
            }


            if (!Between(triangleHL.x, brotherNode.First(), this)
             && !Between(triangleHL.z, brotherNode.First(), this))
            {
                chain.AddLast(brotherNode.First().CreateNode());
                brotherNode.First().SetBrother(chain, groupPoint, triangleHL, sP, fP);
            }
        }
        else
        {

            if (SelfNode.Next == null)
            {
                Debug.Log($"Ha ha next null xD {brotherNode.Count()}");


                Debug.Log($"This: {this}, first: {brotherNode.First()}, last: {brotherNode.Last()}");

                if (!Between(triangleHL.x, brotherNode.First(), this)
                 && !Between(triangleHL.z, brotherNode.First(), this))
                {
                    chain.AddLast(brotherNode.First().CreateNode());
                    brotherNode.First().SetBrother(chain, groupPoint, triangleHL, sP, fP);
                }
                else
                {
                    if (Between(triangleHL.x, brotherNode.First(), this))
                        chain.AddLast(new MarchingPoint(sP));
                    if (Between(triangleHL.z, brotherNode.First(), this))
                        chain.AddLast(new MarchingPoint(fP));
                }
            }
                

            if (SelfNode.Previous == null)
            {
                Debug.Log($"Ha ha prev null xD {brotherNode.Count()}");

                Debug.Log($"This: {this}, first: {brotherNode.First()}, last: {brotherNode.Last()}");

                if (!Between(triangleHL.x, brotherNode.First(), this)
                 && !Between(triangleHL.z, brotherNode.First(), this))
                {
                    chain.AddFirst(brotherNode.First().CreateNode());
                    brotherNode.First().SetBrother(chain, groupPoint, triangleHL, sP, fP);
                }
                else
                {
                    if(Between(triangleHL.x, brotherNode.First(), this))
                        chain.AddFirst(new MarchingPoint(sP));
                    if(Between(triangleHL.z, brotherNode.First(), this))
                        chain.AddFirst(new MarchingPoint(fP));
                }
            }
                
        }

    }

    public LinkedListNode<MarchingPoint> CreateNode()
    {
        SelfNode = new LinkedListNode<MarchingPoint>(this);
        return SelfNode;
    }

    static public bool Between(int triangleID, MarchingPoint firstPoint, MarchingPoint lastPoint)
    {
        return firstPoint.triangleID.Contains(triangleID) && lastPoint.triangleID.Contains(triangleID);
    }

    public void SetColor()
    {

        foreach (var a in edges)
        {
            Debug.DrawLine(a.v0.Position, a.v1.Position, color, 300f);
        }

    }

    public override string ToString()
    {
        return $"IntersectionPoint <T1: {triangleID[0]}, T2: {triangleID[1]}, pos: {position}>";
    }

    public override bool Equals(object obj)
    {
        return obj is MarchingPoint point &&
               EqualityComparer<int[]>.Default.Equals(triangleID, point.triangleID);
    }

    public override int GetHashCode()
    {
        return 645295904 + EqualityComparer<int[]>.Default.GetHashCode(triangleID);
    }
}