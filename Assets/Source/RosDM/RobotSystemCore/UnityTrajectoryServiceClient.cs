using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.MyPkg;
using System.Linq;
using thelab.mvc;

public class UnityTrajectoryServiceClient : Element<DemonOLPApplication>
{
    public TrajectoryServiceRequest request = new TrajectoryServiceRequest();

    public bool response;

    public Vector3[] pose;
    public Quaternion[] rotation;


    public string serviceName = "HMI_srv";

    public void SendMsg()
    {
        if(app.model.Trajectories.Count != 0)
        {
            pose = (from p in app.model.Trajectories.First().GetPositions()
                    select p).ToArray();

            rotation = app.model.Trajectories.First().GetRotations();
        }

        var poses = from p in pose
                    from r in rotation
                    let rp = RosSharp.TransformExtensions.Unity2Ros(new Vector3(p.x, p.y, p.z))
                    let rq = RosSharp.TransformExtensions.Unity2Ros(new Vector3(r.x, r.y, r.z))
                    select new RosSharp.RosBridgeClient.MessageTypes.Geometry.Pose
                    {
                        position = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Point(rp.x, rp.y, rp.z),
                        orientation = new RosSharp.RosBridgeClient.MessageTypes.Geometry.Quaternion(rq.x, rq.y, rq.z, r.w)
                    };

        request = new TrajectoryServiceRequest(1, poses.ToArray());

        GetComponent<RosSharp.RosBridgeClient.RosConnector>()
            .RosSocket.CallService<TrajectoryServiceRequest, TrajectoryServiceResponse>(serviceName, ServiceHandler, request);
    }

    //Handler
    private void ServiceHandler(TrajectoryServiceResponse message)
    {
        response = message.answer;
        Debug.Log("IsMessageAccept: " + message.answer);
    }


}
