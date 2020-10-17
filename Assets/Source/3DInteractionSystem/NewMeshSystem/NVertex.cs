using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NVertex : IEquatable<NVertex>
{
    [SerializeField]
    private int index;
    public int Index { set { index = value; } get => index; }

    
    private int meshId;
    public int MeshId { set { meshId = value; } get => meshId; }

    public Vector3 Position
    {
        get { return NMeshPool.GetMesh(MeshId).vertices[Index]; }
        set { NMeshPool.GetMesh(MeshId).vertices[Index] = value; }
    }

    public override string ToString()
    {
        return $"Index: {Index}, Position: {Position}";
    }

    public bool Equals(NVertex other)
    {
        if (this.Position == other.Position)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
