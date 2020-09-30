using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.HaoAsakura;
using RosSharp.RosBridgeClient;

public class ScannerClientService : MonoBehaviour
{
    //public PolygonModelRequest request = new PolygonModelRequest();
    //public string serviceName = "/add_two_ints";
    //public RosConnector rosConnector;

    //public void StartScann()
    //{
    //    rosConnector.RosSocket.CallService<PolygonModelRequest, PolygonModelResponse>(serviceName, ServiceHandler, request);
    //}

    ////Handler
    //private static void ServiceHandler(PolygonModelResponse message)
    //{
    //    Debug.Log($"GetModel: triangleSize[{message.triangles.Length}], verticesSize[{message.vertices.Length}]");
    //    GameLog.Log($"GetModel: triangleSize[{message.triangles.Length}], verticesSize[{message.vertices.Length}]");
    //}

}