using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Vector6
{
    public float x, y, z;
    public float a, b, c;

    public Vector6(Vector3 xyz, Vector3 abc)
    {
        x = xyz.x;
        y = xyz.y;
        z = xyz.z;

        a = abc.x;
        b = abc.y;
        c = abc.z;
    }
}
