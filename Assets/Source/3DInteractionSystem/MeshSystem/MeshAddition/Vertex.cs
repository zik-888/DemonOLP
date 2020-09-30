using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MeshSystem
{
    public struct Vertex
    {
        public int Index;

        private int _meshId;
        public int MeshId
        {
            get { return _meshId; }
            internal set { _meshId = value; }
        }

        public Vector3 Position
        {
            get { return CustomMeshPool.GetMesh(_meshId).vertices[Index]; }
            set { CustomMeshPool.GetMesh(_meshId).vertices[Index] = value; }
        }

        public static bool operator ==(Vertex c1, Vertex c2)
        {
            if (c1.Position == c2.Position)
                return true;
            else
                return false;
        }

        public static bool operator !=(Vertex c1, Vertex c2) => !(c1 == c2);

        public override bool Equals(object obj) => this == (Vertex)obj;


        public override string ToString() => "Vertex " + Index.ToString() + ": " + Position.ToString();

        public override int GetHashCode()
        {
            return -425505606 + EqualityComparer<Vector3>.Default.GetHashCode(Position);
        }
    }

    class Vector3Comparer : IEqualityComparer<Vector3>
    {
        //public bool Equals(Vector3 point1, Vector3 point2)
        //{
        //    if (point1.x == point2.x 
        //     && point1.y == point2.y 
        //     && point1.z == point2.z )
        //        return true;
        //    else
        //        return false;
        //}

        //public bool Equals(Vector3 point1, Vector3 point2)
        //{
        //    if (point1.x == point2.x
        //     && point1.y == point2.y
        //     && point1.z == point2.z)
        //        return true;
        //    else
        //        return false;
        //}

        //public int GetHashCode(Vector3 point)
        //{
        //    unchecked
        //    {
        //        var hashCode = -731044709;
        //        hashCode = hashCode * -1521134295 + point.x.GetHashCode() 
        //                                          + point.y.GetHashCode() 
        //                                          + point.z.GetHashCode();
        //        return hashCode;
        //    }

        //}


        public bool Equals(Vector3 a, Vector3 b) { return a == b; }
        public int GetHashCode(Vector3 a) { return a.GetHashCode(); }
    }
}