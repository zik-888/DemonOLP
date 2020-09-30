using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshSystem;
using System.Linq;
using thelab.mvc;

/// <summary>
/// Класс инкапсулирующий инициализацию и логику поведения вершин у мешей
/// 
/// Проблемы:
/// 1. Вершин много, большие затраты при инициализации.
/// 2. Возможно выбрана не лучшая архитектура и эти задачи есть смысл 
/// отдать отдельному классу VertexObject агрегированному в Mesh, хотя это имеет свои минусы (спорно).
/// </summary>
public class PointHighlight : Element<DemonOLPApplication>
{
    /// <summary>
    /// Mesh position
    /// </summary>
    public Vector3 positionComponent = Vector3.zero; 

    public float pointSize = 1.5f;

    protected GameObject[] pointGO;

    public void Init(in Vector3[] inputPointArray)
    {
        pointGO = new GameObject[inputPointArray.Count()];

        StartCoroutine(LoadInit(inputPointArray));
    }

    public IEnumerator LoadInit(Vector3[] inputPointArray)
    {
        for (int i = 0; inputPointArray.Count() > i; i++)
        {
            yield return null;

            pointGO[i] = Instantiate(app.model.vertexPoint, gameObject.transform);
            pointGO[i].transform.position = inputPointArray[i];
            pointGO[i].name = i.ToString();
        }
    }

    private void Update()
    {

        // отслеживание зума камеры каждый фрейм для коррекции размера вершины
        try
        {
            if (pointGO != null)
                foreach (var a in pointGO)
                {
                    a.transform.localScale = Vector3.Distance(Camera.main.gameObject.transform.position,
                                                              positionComponent) * Vector3.one * pointSize / 100;
                }
        }
        catch
        {

        }

        
    }



}
