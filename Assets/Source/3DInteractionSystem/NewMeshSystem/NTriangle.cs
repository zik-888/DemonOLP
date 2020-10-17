using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NTriangle
{
    [SerializeField]
    private int _meshId;
    public int MeshId
    {
        get { return _meshId; }
        internal set { _meshId = value; }
    }

    [SerializeField]
    private int _index;

    [SerializeField]
    private int[] idsEdge = new int[3] { -1, -1, -1 };
    

    public NTriangle(int meshId, int index, int v0, int v1, int v2)
    {
        _meshId = meshId;
        _index = index;

        var Edges = new NEdge[] { new NEdge(v0, v1, index, meshId, AddEdge), 
                                  new NEdge(v0, v2, index, meshId, AddEdge), 
                                  new NEdge(v1, v2, index, meshId, AddEdge) };


    }

    public void AddEdge(NEdge nEdge)
    {
        for(int i = 0; idsEdge.Length > i; i++)
        {
            if (idsEdge[i] == -1)
            {
                idsEdge[i] = nEdge.edgeId;
                break;
            }

            if (i == idsEdge.Length - 1)
            {
                throw new Exception("Переполнение idsEdge");
            }
                
        }
    }
}
