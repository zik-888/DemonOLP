using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


namespace MeshSystem
{
    public class CustomMesh
    {
        public int Id;

        /// ....................................

        public Triangle[] Triangles;

        public List<Edge> vector2Ints = new List<Edge>();

        public Vector3[] vertices;

        /// ....................................

        public Mesh selfMesh;

        public int CountPlane { internal set; get; } = 0;

        public List<Surface> surfaceArray = new List<Surface>();

        public CustomMesh(Vector3[] vertices, int[] triangles)
        {
            this.vertices = vertices;

            Id = CustomMeshPool.GetAvailableIndex();
            CustomMeshPool.Push(this);

            Triangles = new Triangle[triangles.Length / 3];

            Triangles = Triangles
                    .AsParallel()
                    .Select((t, i) => new Triangle(Id, i, triangles[i * 3], triangles[i * 3 + 1], triangles[i * 3 + 2]))
                    .ToArray();

            //for(int i = 0; Triangles.Length > i; i++)
            //{
            //    var temp = new Edge(triangles[i * 3], triangles[i * 3 + 1]);

            //    new Edge()

            //    if (vector2Ints.Contains(temp))
            //        vector2Ints.Add();

            //    vector2Ints.Add(new Vector2Int(triangles[i * 3], triangles[i * 3 + 2]));

            //    vector2Ints.Add(new Vector2Int(triangles[i * 3 + 1], triangles[i * 3 + 2]));
            //}

            foreach (var a in Triangles) // инициализация плоскостей
                if (a.IndexPlane == -1)
                    a.SetPlane();


            foreach (var a in surfaceArray)
            {
                //if (a._index == 1)
                //a.SetColorDebuging();
            }

        }

        public void SetInteractivePoints(PointHighlight pointHighlight)
        {
            var vertexArray = from surface in surfaceArray.AsParallel()
                              from vIndex in surface.GetVertices()
                              select vertices[vIndex];

            vertexArray = vertexArray.Distinct();

            //var vertexArray = from surface in surfaceArray
            //                  from vIndex in surface.GetVertices()
            //                  select new VertexPointStruct()
            //                  {
            //                      indexSurface = surface.Index,
            //                      indexVertex = vIndex,
            //                      position = vertices[vIndex]
            //                  };

            //var fooj = vertexArray.GroupBy(v => v.position);



            pointHighlight.Init(vertexArray.ToArray());
        }

        public Vector3 GetPositionVertexByNumber(int vertexNumber)
        {
            return vertices[vertexNumber];
        }

        /// <summary>
        /// Возврат триугольников, принадлежащих поверхностям
        /// </summary>
        public Triangle[] GetTrianglesInSurface(int[] trianglesNum)
        {
            int[] surfaceIndexArray = new int[trianglesNum.Length];

            for(int i = 0; trianglesNum.Length > i; i++)
            {
                surfaceIndexArray[i] = Triangles[trianglesNum[i]].IndexPlane;
            }

            #region Test
            foreach (var a in surfaceIndexArray)
            {
                //Debug.Log($"Surface list: {a}");
                surfaceArray[a].SetColorTriangle();
            }
            #endregion

            var trianglesInSurface = from sIndex in surfaceIndexArray.Distinct().AsParallel()
                                     from tIndex in surfaceArray[sIndex].TriangleIndices
                                     select Triangles[tIndex];

            return trianglesInSurface.ToArray();
        }



        static async public Task<Triangle[]> InitCustomMeshAsync(int[] triangles, int id)
        {
            Triangle[] Triangles = await Task.Run(() => InitCustomMesch(triangles, id));

            return Triangles;
        }

        static protected Triangle[] InitCustomMesch(int[] triangles, int id)
        {
            Triangle[] Triangles = new Triangle[triangles.Length / 3];

            Triangles = Triangles
                    .AsParallel()
                    .Select((t, i) => new Triangle(id, i, triangles[i * 3], triangles[i * 3 + 1], triangles[i * 3 + 2]))
                    .ToArray();

            foreach (var a in Triangles) // инициализация плоскостей
                if (a.IndexPlane == -1)
                    a.SetPlane();

            return Triangles;
        }
    }
}

