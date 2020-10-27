using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrajectory
{
    Vector3[] GetPositions();
    Quaternion[] GetRotations();
}
