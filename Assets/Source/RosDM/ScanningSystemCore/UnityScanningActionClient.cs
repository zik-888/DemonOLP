using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.ScanningSystemCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using thelab.mvc;
using Dummiesman;
using MeshSystem;

[RequireComponent(typeof(RosConnector))]
public class UnityScanningActionClient : Element<DemonOLPApplication>
{
    private RosConnector rosConnector;

    [SerializeField] 
    private ScanningActionClient scanningActionClient;

    public string actionName;

    public ScanningGoalUnity order = new ScanningGoalUnity(5f, 50.5f, 10f, 5f, "METAL");


    public string Status => scanningActionClient.GetStatusString();
    public string Feedback => scanningActionClient.GetFeedbackString();
    public string Result => scanningActionClient.GetResultString();

    private void Start()
    {
        rosConnector = GetComponent<RosConnector>();
        scanningActionClient = new ScanningActionClient(actionName, rosConnector.RosSocket, InitializedModel);
        scanningActionClient.Initialize();
    }

    public void SendGoal()
    {
        RegisterGoal();
        scanningActionClient.SendGoal();
    }


    private void RegisterGoal()
    {
        scanningActionClient.order.x1 = order.x1;
        scanningActionClient.order.x2 = order.x2;
        scanningActionClient.order.y1 = order.y1;
        scanningActionClient.order.y2 = order.y2;
    }

    public void CancelGoal()
    {
        scanningActionClient.CancelGoal();
    }

    public void InitializedModel(Vector3[] vertices, int[] triangles)
    {

        app.model.CurrentLoadScannModel.vertices = vertices;
        app.model.CurrentLoadScannModel.triangles = triangles;
        app.model.CurrentLoadScannModel.normals = CustomMesh.GetNormals(vertices, triangles);


        GameObject gameObject = new GameObject("ScannModel", typeof(MeshFilter), typeof(MeshRenderer));
        gameObject.GetComponent<MeshFilter>().mesh = app.model.CurrentLoadScannModel;
    }
}
