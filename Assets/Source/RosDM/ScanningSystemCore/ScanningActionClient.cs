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
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Shape;
using UniRx;

public enum StatusOfSystem
{
    INIT_SYSTEM,
    READY_TO_SCAN,
    GO_TO_TARGET_POINT,
    SCANNING,
    SCANNING_SUCCESS,
    ERROR_SENDING_TRAJECTORY,
    ERROR_GET_ROBOT_POSITION,
    ERROR_CONNECTION_TO_SCANNER
    // ... e.t.
};

[Serializable]
public class ScanningActionClient : ActionClient<ScanningAction, ScanningActionGoal, ScanningActionResult,
                                                 ScanningActionFeedback, ScanningGoal, ScanningResult, ScanningFeedback>
{
    public ScanningGoal order;
    public ReactiveProperty<ScanningResult> reactOrder { set; get; } = new ReactiveProperty<ScanningResult>();
    public string status = "";
    public string feedback = "";
    public string result = "";

    public ScanningActionClient(string actionName, RosSocket rosSocket)
    {
        this.actionName = actionName;
        this.rosSocket = rosSocket;

        action = new ScanningAction();
        goalStatus = new GoalStatus();
        order = new ScanningGoal();
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
        reactOrder.Value = action.action_result.result;
        //actionResult(action.action_result.result.vertices, action.action_result.result.triangles);
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
            return string.Join(",", (StatusOfSystem)action.action_feedback.feedback.status);
        return "";
    }

    public string GetResultString()
    {
        if (action != null)
            return string.Join(",", action.action_result.result);
        return "";
    }
}
