using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using thelab.mvc;
using UnityEngine;


/// <summary>
/// Класс, описывающий опорную точку при марчинге.
/// Главная особенность - возможность перемещения по поверхности модели методом drug&drop
/// </summary>

public class ReferencePoint : Element<DemonOLPApplication>
{
    public Action useMethod { set; get; }
    public int triangleIndex;

    private Color defColor;

    private void OnMouseEnter()
    {
        defColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = defColor;
    }

    private void OnMouseDrag()
    {
        bool interact = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);

        print($"Interact: {interact}, Body: {hit.collider}");

        if(app.model.meshModelNameArray.Any(s => s == hit.collider.name))
        {
            gameObject.transform.position = hit.point;
            triangleIndex = hit.triangleIndex;
        }
            
    }

    private void OnMouseUpAsButton()
    {
        try
        {
            useMethod();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        
    }
}
