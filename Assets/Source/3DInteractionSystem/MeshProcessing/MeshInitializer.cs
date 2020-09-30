using MeshSystem;
using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;
using System.Threading.Tasks;
using UniRx;

/// <summary>
/// Класс инициализирует модель меша.
/// Может инициализировать подсветку вершин
/// </summary>
public class MeshInitializer : Element<DemonOLPApplication>
{
    public CustomMesh SelfMesh { set; get; }
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
        Mesh protoMesh = gameObject.GetComponent<MeshFilter>().mesh;

        if (IsHLVertexWork == true)
            highlighter = gameObject.AddComponent<PointHighlight>();

        SelfMesh = await InitMeshAsync(protoMesh.vertices, protoMesh.triangles);
        //SelfMesh = new CustomMesh(protoMesh.vertices, protoMesh.triangles);

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
}
