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

            foreach (var a in Triangles) // инициализация плоскостей
                if (a.IndexPlane == -1)
                    a.SetPlane();


            foreach (var a in surfaceArray)
            {

                a.SetNormal();
                //if (a._index == 1)
                //a.SetColorDebuging();
            }

        }

        public Vector3[] GetSurfaceNormals()
        {
            return surfaceArray.Select(s => s.surfaceNormal).ToArray();
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
            Triangle[] Triangles = await Task.Run(() => InitCustomMesh(triangles, id));

            return Triangles;
        }

        static protected Triangle[] InitCustomMesh(int[] triangles, int id)
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


        /// <summary>
        /// Расчет нормалей
        /// </summary>
        /// <param name="vertices">Набор координат точек</param>
        /// <param name="faces">Набор индексов точек, объединямых 
        /// в треугольники</param>
        /// <returns></returns>
        public static Vector3[] GetNormals(Vector3[] vertices, int[] faces)
        {
            // Массив нормалей
            var normals = new Vector3[vertices.Length];

            // Инициализация нулями
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = new Vector3(0, 0, 0);
            }

            // Перебор всех треугольников
            for (int i = 0; i < faces.Length; i += 3)
            {
                float nx = float.NaN, ny = float.NaN, nz = float.NaN;
                int counter = 0;

                Vector3 a, b, c;

                // Возможные комбинации следования точек в треугольнике
                var ind = new[] {
                                  i, i + 1, i + 2,
                                  i, i + 2, i + 1,
                                  i + 1, i, i + 2,
                                  i + 1, i + 2, i,
                                  i + 2, i, i +1,
                                  i + 2, i + 1, i
                                };

                int indOffset = 0;

                // Расчет нормали если она не была расчитана на предыдущей 
                // итерации
                while (float.IsNaN(nx) && float.IsNaN(ny) && float.IsNaN(nz) && counter < 6)
                {
                    a = vertices[faces[ind[indOffset + 0]]];
                    b = vertices[faces[ind[indOffset + 1]]];
                    c = vertices[faces[ind[indOffset + 2]]];

                    var A = a - b;
                    var B = b - c;

                    var Nx = A.y * B.z - A.z * B.y;
                    var Ny = A.z * B.x - A.x * B.z;
                    var Nz = A.x * B.y - A.y * B.x;

                    var len = (float)Math.Sqrt(Nx * Nx + Ny * Ny + Nz * Nz);

                    nx = Nx / len;
                    ny = Ny / len;
                    nz = Nz / len;

                    indOffset += 3;
                    counter += 1;
                }

                var fn = new Vector3(nx, ny, nz);

                normals[faces[ind[indOffset]]] = fn;
                normals[faces[ind[indOffset + 1]]] = fn;
                normals[faces[ind[indOffset + 2]]] = fn;
            }

            return normals;
        }

    }
}

