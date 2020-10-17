using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;

[Serializable]
public class NEdge : IEquatable<NEdge>, IEqualityComparer<NEdge>
{
    public int edgeId;

    public NVertex v0 = new NVertex();

    public NVertex v1 = new NVertex();

    public int MeshId { get; internal set; }


    [SerializeField]
    private int[] triangleIndexes = new int[2] { -1, -1 };

    public int[] TriangleIndexes { get { return triangleIndexes; } set { triangleIndexes = value; } }

    public void AddTriangle(int triangleIndex)
    {
        for (int i = 0; triangleIndexes.Length > i; i++)
        {
            if(triangleIndexes[i] == -1)
            {
                triangleIndexes[i] = triangleIndex;
                break;
            }

            //if (i == triangleIndexes.Length - 1)
            //{
            //    throw new Exception("Переполнение triangleIndexes");
            //}
        }
    }

    public NEdge(int first, int triangleIndex, int second, int meshId, Action<NEdge> addEdge)
    {
        MeshId = meshId;
        v0.Index = first;
        v1.Index = second;


        if (NMeshPool.GetMesh(meshId).Edges.Contains(this)) // если едж содержится в списке
        {

            var edge = NMeshPool.GetMesh(meshId).Edges.Find(e => e.Equals(this));
            //edge.AddTriangle(triangleIndex);
            addEdge(NMeshPool.GetMesh(meshId).Edges[edge.edgeId]);
        }
        else // если эдж не содержится в списке
        {
            //AddTriangle(triangleIndex);

            edgeId = NMeshPool.GetMesh(meshId).Edges.Count;
            NMeshPool.GetMesh(meshId).Edges.Add(this);

            addEdge(this);

            //Debug.DrawLine(v0.Position, v1.Position, new Color(UnityEngine.Random.value,
            //                                                   UnityEngine.Random.value,
            //                                                   UnityEngine.Random.value, 1), 1000f);

        }

        //Debug.Log($"First {first}, second {second}");
    }

    public override string ToString()
    {
        return $"v0: {v0}, v1: {v1}";
    }

    public bool Equals(NEdge other)
    {
        if (v0.Equals(other.v0) && v1.Equals(other.v1) ||
            v0.Equals(other.v1) && v1.Equals(other.v0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Equals(NEdge x, NEdge y)
    {
        if (x.v0.Equals(y.v0) && x.v1.Equals(y.v1) ||
            x.v0.Equals(y.v1) && x.v1.Equals(y.v0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        int hashCode = -731044709;
        hashCode = hashCode + EqualityComparer<Vector3>.Default.GetHashCode(v0.Position);
        hashCode = hashCode + EqualityComparer<Vector3>.Default.GetHashCode(v1.Position);
        return hashCode;
    }

    public int GetHashCode(NEdge obj)
    {
        int hashCode = -731044709;
        hashCode = hashCode + EqualityComparer<Vector3>.Default.GetHashCode(obj.v0.Position);
        hashCode = hashCode + EqualityComparer<Vector3>.Default.GetHashCode(obj.v1.Position);
        return hashCode;
    }
}

