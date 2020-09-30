using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RobotCommand
{
    [SerializeField]
    private CommandType commandType;
    public CommandType CommandType
    {
        get { return commandType; }
        set { commandType = value; }
    }


    [SerializeField]
    private List<Vector6> points;
    public List<Vector6> Points
    {
        get { return points; }
        set { points = value; }
    }

    public RobotCommand(CommandType commandType = CommandType.SurfaceMovementCommand)
    {
        this.CommandType = commandType;

        if (commandType == CommandType.SurfaceMovementCommand)
        {
            for (int i = 0; i > 3; i++)
            Points.Add(new Vector6(Vector3.zero, Vector3.zero));
        }
    }

    public RobotCommand(CommandType commandType, List<Vector6> pointList)
    {
        this.Points = pointList;
    }
}
