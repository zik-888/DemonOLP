using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MeshSystem
{
    public class Triangle
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_edges != null)
                {
                    _edges[0].TriangleIndex = value;
                    _edges[1].TriangleIndex = value;
                    _edges[2].TriangleIndex = value;
                }
            }
        }

        public int IndexPlane { private set; get; } = -1;
        public Vector3 Normal { internal set; get; } = Vector3.zero;

        public float AnglePlaneForm { internal set; get; } = 20f;

        private int _meshId;
        public int MeshId
        {
            get { return _meshId; }
            internal set { _meshId = value; }
        }

        private Edge[] _edges;

        public Edge[] Edges
        {
            get { return _edges; }
            set
            {
                if (value.Length == 3)
                {
                    _edges = value;
                    for (var i = 0; i < 3; i++)
                    {
                        _edges[i].TriangleIndex = _index;
                    }
                }
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public Vertex V0
        {
            get { return Edges[0].v0; }
            set
            {
                if (value.MeshId != MeshId)
                    throw new Exception("Not the same mesh");

                Edges[0].v0 = value;
                Edges[2].v1 = value;
            }
        }

        public Vertex V1
        {
            get { return Edges[1].v0; }
            set
            {
                if (value.MeshId != MeshId)
                    throw new Exception("Not the same mesh");

                Edges[1].v0 = value;
                Edges[0].v1 = value;
            }
        }

        public Vertex V2
        {
            get { return Edges[2].v0; }
            set
            {
                if (value.MeshId != MeshId)
                    throw new Exception("Not the same mesh");

                Edges[2].v0 = value;
                Edges[1].v1 = value;
            }
        }

        public Vertex[] Vertices()
        {
            return new Vertex[] { V0, V1, V2 };
        }

        public Triangle(int meshId, int index, int v0, int v1, int v2)
        {
            _index = index;
            _meshId = meshId;

            var edges = new Edge[3];
            edges[0] = new Edge(meshId, index, v0, v1);
            edges[1] = new Edge(meshId, index, v1, v2);
            edges[2] = new Edge(meshId, index, v2, v0);

            _edges = edges;

            Normal = PlaneNormal(); // находим нормаль к треугольнику
        }

        /// <summary>
        /// Устанавливает 3 соседних треугольника
        /// </summary>
        /// <returns></returns>
        private int[] SetNeighbor()
        {
            var neighborTriangle = from t in CustomMeshPool.GetMesh(_meshId).Triangles.AsParallel()
                                   where t.Index != Index
                                   from e1 in Edges
                                   where t.Edges.Any(e2 => e2 == e1)
                                   select t.Index;

            //string s = Index.ToString() + " neighborTriangle: ";

            //foreach(var a in neighborTriangle)
            //{
            //    s += " " + a.ToString();
            //}

            //Debug.Log(s);

            return neighborTriangle.ToArray();
        }

        /// <summary>
        /// Поиск среди соседних треугольников ...
        /// </summary>
        /// <param name="angle"></param>
        private int[] SetPlaneForm(float angle)
        {
            int[] neighborTriangle = SetNeighbor();

            var neighborTrianglePlaneForm = from nT in neighborTriangle
                                            where Vector3.Angle(Normal, CustomMeshPool.GetMesh(_meshId).Triangles[nT].Normal) < angle
                                            select nT;

            //string s = Index.ToString() + " neighborTrianglePlaneForm: ";

            //foreach (var a in neighborTrianglePlaneForm)
            //{
            //    s += " " + a.ToString();
            //}

            //Debug.Log(s);

            return neighborTrianglePlaneForm.ToArray();
        }
        /// <summary>
        /// Устанавливает какой плоскости принадлежит треугольник
        /// </summary>
        protected internal void SetPlane()
        {
            int[] neighborTrianglePlaneForm = SetPlaneForm(AnglePlaneForm);

            if (IndexPlane == -1)
            {
                CustomMeshPool.GetMesh(_meshId).surfaceArray.Add(new Surface(MeshId, CustomMeshPool.GetMesh(_meshId).surfaceArray.Count));
                IndexPlane = CustomMeshPool.GetMesh(_meshId).surfaceArray.Count - 1;
                CustomMeshPool.GetMesh(_meshId).surfaceArray[IndexPlane].AddTriangle(Index);
            }

            foreach (var nT in neighborTrianglePlaneForm)
            {
                if (CustomMeshPool.GetMesh(_meshId).Triangles[nT].IndexPlane == -1)
                {
                    CustomMeshPool.GetMesh(_meshId).Triangles[nT].IndexPlane = IndexPlane;
                    CustomMeshPool.GetMesh(_meshId).surfaceArray[IndexPlane].AddTriangle(nT);
                    CustomMeshPool.GetMesh(_meshId).Triangles[nT].SetPlane();
                }
            }
        }

        /// <summary>
        /// Возвращает вектор нормали к плоскости треугольника
        /// </summary>
        private Vector3 PlaneNormal()
        {
            Vector3 point1 = V0.Position;
            Vector3 point2 = V1.Position;
            Vector3 point3 = V2.Position;

            //Vector3 planeParam = new Vector3(-point2.y * point1.z + point3.y * point1.z + point1.y * point2.z - point3.y * point2.z - point1.y * point3.z + point2.y * point3.z,
            //                                  point2.x * point1.z - point3.x * point1.z - point1.x * point2.z + point3.x * point2.z + point1.x * point3.z - point2.x * point3.z,
            //                                  point3.x * (point1.y - point2.y) + point1.x * (point2.y - point3.y) + point2.x * (-point1.y + point3.y));

            Vector3 planeParam = Vector3.Cross(point2 - point1, point3 - point1).normalized;

            return new Vector3(planeParam.x, planeParam.y, planeParam.z);
        }

        public void DrawNormal()
        {
            Vector3 centerTriangle = (V0.Position + V1.Position + V2.Position) / 3;
            Debug.DrawRay(centerTriangle, Normal, Color.cyan, 1000f);
        }

        public override string ToString()
        {
            return Index.ToString() + ": {" + Edges[0].ToString() + " " + Edges[1].ToString() + " " + Edges[2].ToString() + "}";
        }

        public override bool Equals(object obj)
        {
            return obj is Triangle triangle &&
                   MeshId == triangle.MeshId &&
                   EqualityComparer<Vertex>.Default.Equals(V0, triangle.V0) &&
                   EqualityComparer<Vertex>.Default.Equals(V1, triangle.V1) &&
                   EqualityComparer<Vertex>.Default.Equals(V2, triangle.V2);
        }

        public override int GetHashCode()
        {
            var hashCode = -1624152114;
            hashCode = hashCode * -1521134295 + _index.GetHashCode();
            hashCode = hashCode * -1521134295 + Index.GetHashCode();
            hashCode = hashCode * -1521134295 + IndexPlane.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(Normal);
            hashCode = hashCode * -1521134295 + AnglePlaneForm.GetHashCode();
            hashCode = hashCode * -1521134295 + _meshId.GetHashCode();
            hashCode = hashCode * -1521134295 + MeshId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Edge[]>.Default.GetHashCode(_edges);
            hashCode = hashCode * -1521134295 + EqualityComparer<Edge[]>.Default.GetHashCode(Edges);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(V0);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(V1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(V2);
            return hashCode;
        }

        public static bool operator ==(Triangle c1, Triangle c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Triangle c1, Triangle c2)
        {
            return !c1.Equals(c2);
        }
    }
}
