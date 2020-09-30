using UnityEngine;
using System.Collections;
using rosapi = RosSharp.RosBridgeClient.MessageTypes.Rosapi;
using RosSharp.RosBridgeClient;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.RosBookSamples;

public class TestServerService : UnityServiceProvider<T_Request, AddTwoIntsResponse>
{

    protected override void Start()
    {
        base.Start();
        Debug.Log("Hello Server Service");
        //GetComponent<RosConnector>().RosSocket.UnadvertiseService(ServiceId);
    }
    protected override bool ServiceCallHandler(T_Request request, out AddTwoIntsResponse response)
    {
        Debug.Log((request.x + request.y).ToString());
        response = new AddTwoIntsResponse(request.x + request.y);
        return true;
    }
}


