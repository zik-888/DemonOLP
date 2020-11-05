using MeshSystem;
using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;
using System.Threading.Tasks;
using UniRx;
using System.IO;
using System;
using System.Linq;

/// <summary>
/// Класс инициализирует модель меша.
/// Может инициализировать подсветку вершин
/// </summary>
public class MeshInitializer : Element<DemonOLPApplication>
{
    public CustomMesh SelfMesh { set; get; }

    public NMesh nMesh;

    protected PointHighlight highlighter;

    protected bool isHLVertexWork = true;
    public bool IsHLVertexWork
    {
        get => isHLVertexWork;

        set
        {
            switch (value)
            {
                case true:
                    if (isHLVertexWork == false) 
                    {
                        isHLVertexWork = true;
                        highlighter = gameObject.AddComponent<PointHighlight>();
                    }
                    break;
                case false:
                    if (isHLVertexWork == true)
                    {
                        isHLVertexWork = false;
                        Destroy(highlighter);
                    }
                    break;
            }
        }
    }

    private async void Awake()
    {
        print("GoMeshInit");
        Mesh protoMesh = gameObject.GetComponent<MeshFilter>().mesh;

        if (IsHLVertexWork == true)
            highlighter = gameObject.AddComponent<PointHighlight>();

        SelfMesh = await InitMeshAsync(protoMesh.vertices, protoMesh.triangles);

        //await StartCoroutine(TestCoroutine());

        //SelfMesh = new CustomMesh(protoMesh.vertices, protoMesh.triangles);


        //new NEdge()


        //nMesh = new NMesh(protoMesh.vertices, protoMesh.triangles);
    }


    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(1f);

        var a = new Vector3[] { new Vector3(1, 1, -1), new Vector3(-1, 1, -1), new Vector3(-1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, -1, 1), new Vector3(1, 1, 1), new Vector3(-1, 1, 1), new Vector3(-1, -1, 1), new Vector3(-1, -1, 1), new Vector3(-1, 1, 1), new Vector3(-1, 1, -1), new Vector3(-1, -1, -1), new Vector3(-1, -1, -1), new Vector3(1, -1, -1), new Vector3(1, -1, 1), new Vector3(-1, -1, 1), new Vector3(1, -1, -1), new Vector3(1, 1, -1), new Vector3(1, 1, 1), new Vector3(1, -1, 1), new Vector3(-1, -1, -1), new Vector3(-1, 1, -1), new Vector3(1, 1, -1), new Vector3(1, -1, -1) };
        var b = new int[] { 0, 1, 2, 2, 3, 0, 4, 5, 6, 6, 7, 4, 8, 9, 10, 10, 11, 8, 12, 13, 14, 14, 15, 12, 16, 17, 18, 18, 19, 16, 20, 21, 22, 22, 23, 20 };
        //var c = GetComponent<MeshFilter>().mesh.normals;


        Mesh mesh = new Mesh();
        

        mesh.vertices = a;
        mesh.triangles = b;
        //mesh.normals = CustomMesh.GetNormals(a, b);
       //mesh.RecalculateNormals();



        GameObject gameObject = new GameObject("ScannModel", typeof(MeshFilter), typeof(MeshRenderer));
        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        Debug.Log(Time.deltaTime + "Create");
    }

    protected async Task<CustomMesh> InitMeshAsync(Vector3[] vertices, int[] triangles)
    {
        Debug.Log("Start Task");
        app.model.LoadedAmination.Value = true;
            //= new ReactiveProperty<bool>(true);
        var a = await Task.Run(() => new CustomMesh(vertices, triangles));
        app.model.LoadedAmination.Value = false;
        Debug.Log("Finish Task");
        return a;
    }

    protected void SerializeVertexAndTriangle(Vector3 [] vertices, int[] triangles)
    {
        string writePath = @"C:\Users\HaoAsakura\YandexDisk\SS_ISH\Демонстратор\DemonOLP\hta.txt";

        string text = "";

        text += "[ ";
        foreach (var a in vertices)
        {
            text += $"new Vector3({a.x}, {a.y}, {a.z}), ";
        }
        text += " ]";

        text += "\r\n";

        text += "[ ";
        foreach (var a in triangles)
        {
            text += $"{a}, ";
        }
        text += " ]";

        try
        {
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(text);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }

}
