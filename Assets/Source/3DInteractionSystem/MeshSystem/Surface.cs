using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace MeshSystem
{
    /// <summary>
    ///  класс, описывающий поверхность, через набор треугольников
    /// </summary>
    public class Surface
    {
        private int MeshId { set; get; }
        public int Index { set; get; }
        public List<int> TriangleIndices { private set; get; } = new List<int>();

        public Vector3 surfaceNormal;

        protected List<Edge> OutsideEdges { set; get; } = new List<Edge>();


        protected Color color;

        public Surface(int meshId, int index)
        {
            MeshId = meshId;
            Index = index;

            try
            {
                color = new Color(Random.value, Random.value, Random.value, 1);
            }
            catch { /*Debug.Log("Err");*/ }
            
        }

        public void SetNormal()
        {
            

            var b = from tI in TriangleIndices
                    select CustomMeshPool.GetMesh(MeshId).Triangles[tI].Normal;

            var x = b.Sum(n => n.x);
            var y = b.Sum(n => n.y);
            var z = b.Sum(n => n.z);

            int countT = TriangleIndices.Count;

            surfaceNormal = new Vector3(x / countT, y / countT, z / countT);
        }

        public void AddTriangle(int index)
        {
            TriangleIndices.Add(index);
        }

        public int[] GetVertices()
        {
            var edges = from tIndex in TriangleIndices.AsParallel()
                        from edge in CustomMeshPool.GetMesh(MeshId).Triangles[tIndex].Edges
                        select edge;

            edges = edges.AsParallel()
                    .GroupBy(x => x, new EdgeComparer())
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key);

            return edges.Select(e => e.v0.Index).Union(edges.Select(e => e.v1.Index)).Distinct().ToArray();
        }

        public void SetColorDebuging()
        {
            var edges = from tIndex in TriangleIndices.AsParallel()
                        from edge in CustomMeshPool.GetMesh(MeshId).Triangles[tIndex].Edges
                        select edge;

            edges = edges.AsParallel()
                    .GroupBy(x => x, new EdgeComparer())
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key);


            //Debug.Log("Log plane " + Index.ToString());

            foreach (var a in edges)
            {
                //Debug.Log(a.ToString() + a.TriangleIndex.ToString());
                Debug.DrawLine(a.v0.Position, a.v1.Position, color, 300f);
            }

            //var vertexInd = from tIndex in TriangleIndices.AsParallel()
            //                from v in CustomMeshPool.GetMesh(MeshId).Triangles[tIndex].Vertices()
            //                select v.Index;

            //foreach(var a in vertexInd)
            //{
            //    CustomMeshPool.GetMesh(MeshId).selfMesh.colors[a] = color;
            //}

            

        }

        public void SetColorTriangle()
        {

            foreach (var a in TriangleIndices)
            {
                foreach(var b in CustomMeshPool.GetMesh(MeshId).Triangles[a].Edges)
                {
                    Debug.DrawLine(b.v0.Position, b.v1.Position, color, 300f);
                }
            }
        }

        public override string ToString()
        {
            string s = " ";

            foreach (var a in TriangleIndices)
                s += a.ToString() + " ";

            return "Plane " + Index.ToString() + ":" + s;
        }
    }
}

