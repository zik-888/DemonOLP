using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.Actionlib;
using RosSharp.RosBridgeClient.MessageTypes.ScanningSystemCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Timers;
using System.Linq;

[Serializable]
public class ScanningActionClient : ActionClient<ScanningAction, ScanningActionGoal, ScanningActionResult,
                                                 ScanningActionFeedback, ScanningGoal, ScanningResult, ScanningFeedback>
{
    public ScanningGoal order;
    public string status = "";
    public string feedback = "";
    public string result = "";

    private Action<Vector3[], int[]> actionResult;

    public ScanningActionClient(string actionName, RosSocket rosSocket, Action<Vector3[], int[]> actionResult)
    {
        this.actionName = actionName;
        this.rosSocket = rosSocket;

        action = new ScanningAction();
        goalStatus = new GoalStatus();
        order = new ScanningGoal();

        this.actionResult = actionResult;
    }

    protected override ScanningActionGoal GetActionGoal()
    {
        action.action_goal.goal = order;
        return action.action_goal;
    }

    protected override void OnStatusUpdated()
    {
        status = GetStatusString();
    }

    protected override void OnFeedbackReceived()
    {
        feedback = GetFeedbackString();
    }

    protected override void OnResultReceived()
    {

        result = $"Result time: {DateTime.Now}, Length: {action.action_result.result.vertices.Length}";



        var vertex = action.action_result.result.vertices.Select(v => new Vector3((float)v.x, (float)v.y, (float)v.z));

        var triangle = from t in action.action_result.result.triangles
                       from tt in t.vertex_indices
                       select (int)tt;

        actionResult(vertex.ToArray(), triangle.ToArray());
    }

    public string GetStatusString()
    {
        if (goalStatus != null)
        {
            return ((ActionStatus)(goalStatus.status)).ToString();
        }
        return "";
    }

    public string GetFeedbackString()
    {
        if (action != null)
            return string.Join(",", action.action_feedback.feedback.status);
        return "";
    }

    public string GetResultString()
    {
        if (action != null)
            return string.Join(",", action.action_result.result);
        return "";
    }
}
