using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NMesh
{
    public int Id;

    public NTriangle[] Triangles;

    public List<NEdge> Edges = new List<NEdge>();

    //public HashSet<NEdge> nEdges = new HashSet<NEdge>();

    public Vector3[] vertices;


    public NMesh(Vector3[] vertices, int[] triangles)
    {
        this.vertices = vertices;

        Id = NMeshPool.GetAvailableIndex();
        NMeshPool.Push(this);
        
        Triangles = new NTriangle[triangles.Length / 3];

        Triangles = Triangles.AsParallel()
                             .Select((t, i) => new NTriangle(Id, i, triangles[i * 3    ],
                                                                    triangles[i * 3 + 1], 
                                                                    triangles[i * 3 + 2]))
                             .ToArray();

        //Debug.Log($"Count - {Edges.Count}");

        //foreach (var a in Edges)
        //{
            
            

        //    try
        //    {
        //        Debug.Log(a.ToString());
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.LogWarning(e.ToString());
        //    }
        //}

    }
}
