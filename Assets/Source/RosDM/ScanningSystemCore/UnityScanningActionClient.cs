using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.ScanningSystemCore;
using UnityEngine;
using UniRx;
using thelab.mvc;
using System;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Shape;
using System.Linq;
using MeshSystem;

[RequireComponent(typeof(RosConnector))]
public class UnityScanningActionClient : Element<DemonOLPApplication>
{
    private RosConnector rosConnector;

    [SerializeField] 
    private ScanningActionClient scanningActionClient;

    public string actionName;

    public ScanningGoalUnity order = new ScanningGoalUnity(0, -0.25f, 0, 0.325f);


    public string Status => scanningActionClient.GetStatusString();
    public string Feedback => scanningActionClient.GetFeedbackString();
    public string Result => scanningActionClient.GetResultString();

    private void Awake()
    {
        app.model.scannArea = new Vector4(order.x1, order.x2, order.y1, order.y2);
    }

    private void Start()
    {
        rosConnector = GetComponent<RosConnector>();
        scanningActionClient = new ScanningActionClient(actionName, rosConnector.RosSocket);
        scanningActionClient.Initialize();

        scanningActionClient.reactOrder.ObserveEveryValueChanged(x => x.Value)
                                       .Subscribe(xs => InitModel(xs))
                                       .AddTo(this);
        

    }

    public void SendGoal()
    {
        RegisterGoal();
        scanningActionClient.SendGoal();
    }


    private void RegisterGoal()
    {
        //order = new ScanningGoalUnity(app.model.scannArea.x, app.model.scannArea.y,
        //                              app.model.scannArea.z, app.model.scannArea.w);

        scanningActionClient.order.x1 = app.model.scannArea.x;
        scanningActionClient.order.x2 = app.model.scannArea.y;
        scanningActionClient.order.y1 = app.model.scannArea.z;
        scanningActionClient.order.y2 = app.model.scannArea.w;
    }

    public void CancelGoal()
    {
        scanningActionClient.CancelGoal();
    }

    public void InitModel(ScanningResult result)
    {
        if(result != null)
        {
            var vertex = result.vertices.Select(v => new UnityEngine.Vector3(-(float)v.y, (float)v.z, (float)v.x)).ToArray();
            

            //var vertex = result.vertices.Select(v => RosSharp.TransformExtensions.Ros2Unity(
            //                                       new UnityEngine.Vector3((float)v.x, (float)v.y, (float)v.z))).ToArray();

            var triangle = (from t in result.triangles
                            //let n = new int[3] { (int)t.vertex_indices[0], (int)t.vertex_indices[2], (int)t.vertex_indices[1] }
                           from tt in t.vertex_indices
                           select (int)tt).ToArray();

            UnityEngine.Vector3 delta = UnityEngine.Vector3.zero;

            delta.x = vertex.Average(p => p.x);
            delta.y = vertex.Average(p => p.y);
            delta.z = vertex.Average(p => p.z);

            app.model.deltaModel = delta;

            vertex = vertex.Select(v => new UnityEngine.Vector3(v.x, v.y, v.z) /*- delta*/).ToArray();

            UnityEngine.Mesh mesh = new UnityEngine.Mesh();
            mesh.vertices = vertex;
            mesh.triangles = triangle;
            //mesh.normals = CustomMesh.GetNormals(vertex, triangle);
            //mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            //mesh.RecalculateTangents();
            //
            //mesh.Optimize();

            app.model.CurrentLoadScannModel = mesh;
            app.model.IsLoadScannModel = true;
            app.controller.AddMesh();
        }
    }

    public void InitializedModel(Point[] points, MeshTriangle[] faces)
    {

        

        ////app.model.CurrentLoadScannModel.vertices = vertices;
        ////app.model.CurrentLoadScannModel.triangles = triangles;

        //print($"vertices: {points.Length}");
        //print($"triangles: {faces.Length}");

        ////app.model.CurrentLoadScannModel.normals = CustomMesh.GetNormals(vertices, triangles);


        //GameObject gameObject = new GameObject("ScannModel", typeof(MeshFilter), typeof(MeshRenderer));
        //Instantiate(gameObject);
        //gameObject.GetComponent<MeshFilter>().mesh = app.model.CurrentLoadScannModel;

        //UnityEngine.Mesh mesh = new UnityEngine.Mesh();

        //mesh.vertices = vertex.ToArray();
        //mesh.triangles = triangle.ToArray();
        //mesh.normals = CustomMesh.GetNormals(mesh.vertices, mesh.triangles);


    }
}

[Serializable]
public class ScanningGoalUnity
{
    // goal definition

    public float x1;
    public float y1;
    public float x2;
    public float y2;

    public ScanningGoalUnity()
    {
        this.x1 = 0f;
        this.y1 = 0f;
        this.x2 = 0f;
        this.y2 = 0f;
    }

    public ScanningGoalUnity(float x1, float y1, float x2, float y2)
    {
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
    }
}