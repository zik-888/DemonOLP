using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace MeshSystem
{
    public struct Edge : IEquatable<Edge>
    {
        public Vertex v0;

        public Vertex v1;

        public Vertex[] Vertices
        {
            get { return new Vertex[] { v0, v1 }; }
        }

        private int _meshId;
        public int MeshId
        {
            get { return _meshId; }
            internal set { _meshId = value; }
        }

        private int _triangleIndex;
        public int TriangleIndex
        {
            get { return _triangleIndex; }
            internal set { _triangleIndex = value; }
        }

        public Edge(int meshId, int triangleIndex, int v0Index, int v1Index)
        {
            _meshId = meshId;
            _triangleIndex = triangleIndex;

            v0 = new Vertex() { MeshId = meshId, Index = v0Index };

            v1 = new Vertex() { MeshId = meshId, Index = v1Index };
        }

        public static bool GetEqualsArray(Edge[] c1, Edge[] c2)
        {
            int coincidenceCount = 0;

            foreach (var a in c1)
            {
                foreach (var b in c2)
                {
                    if (a == b)
                        coincidenceCount++;
                }
            }

            if (coincidenceCount == 1)
                return true;
            else if (coincidenceCount == 0)
                return false;
            else
                throw new Exception("coincidenceCount > 1");
        }

        public static bool GetEqualsOnTriangle(Edge[] c1, Edge[] c2)
        {
            int coincidenceCount = 0;

            foreach (var a in c1)
            {
                foreach (var b in c2)
                {
                    if (a.TriangleIndex == b.TriangleIndex)
                        coincidenceCount++;
                }
            }

            if (coincidenceCount == 1)
                return true;
            else if (coincidenceCount == 0)
                return false;
            else
                throw new Exception("coincidenceCount > 1");
        }

        public static bool operator ==(Edge c1, Edge c2)
        {
            if ((c1.v0 == c2.v0 && c1.v1 == c2.v1) || (c1.v1 == c2.v0 && c1.v0 == c2.v1))
                return true;
            else
                return false;
        }

        public static bool operator !=(Edge c1, Edge c2) => !(c1 == c2);

        

        public bool Equals(Edge other)
        {
            return this == other;
        }

        public override string ToString()
        {
            return "[" + v0.Index.ToString() + " " + v1.Index.ToString() + "]";
        }

        public override int GetHashCode()
        {
            var hashCode = -731044709;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(v0);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(v1);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return this == (Edge)obj;
        }
    }

    class EdgeComparer : IEqualityComparer<Edge>
    {
        public bool Equals(Edge edge1, Edge edge2)
        {
            if (edge1.v0.Position == edge2.v0.Position && edge1.v1.Position == edge2.v1.Position
             || edge1.v1.Position == edge2.v0.Position && edge1.v0.Position == edge2.v1.Position)
                return true;
            else
                return false;
        }

        public int GetHashCode(Edge edge)
        {
            unchecked
            {
                var hashCode = -731044709;
                hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(edge.v0)
                                                  + EqualityComparer<Vertex>.Default.GetHashCode(edge.v1);
                return hashCode;
            }
            
        }
    }
}
