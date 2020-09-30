using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.RosBookSamples;
using System;

public class TestClientService : MonoBehaviour
{
    public T_Request request = new T_Request(2, 3);
    public string serviceName = "/add_two_ints";

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RosSharp.RosBridgeClient.RosConnector>()
            .RosSocket.CallService<T_Request, AddTwoIntsResponse>(serviceName, ServiceHandler, request);
    }

    //Handler
    private static void ServiceHandler(AddTwoIntsResponse message)
    {
        Debug.Log("Sum: " + message.sum);
    }

}



