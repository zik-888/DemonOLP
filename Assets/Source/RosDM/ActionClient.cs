using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.RosBookSamples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionClient : MonoBehaviour
{
    public string actionName = "do_dishes";

    protected static RosSocket rosSocket;
    protected static FibonacciActionClient fibonacciActionClient;

    // Start is called before the first frame update
    void Start()
    {
        rosSocket = GetComponent<RosConnector>().RosSocket;
        fibonacciActionClient = new FibonacciActionClient(actionName, rosSocket);
        fibonacciActionClient.Initialize();

        

    }

    //Handler
    private static void ServiceHandler(AddTwoIntsResponse message)
    {
        Debug.Log("Sum: " + message.sum);
    }

}
