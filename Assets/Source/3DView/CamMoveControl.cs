using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMoveControl : MonoBehaviour
{
    Camera mainCamera;
    maxCamera mxc;
    float sceenPart = 0.3f;
    float currentDistance;
    bool positionAnObjectAsWork = false;

    private void Start()
    {
        mainCamera = Camera.main;
        mxc = mainCamera.GetComponent<maxCamera>();
    }

    private void Update()
    {
        //print(positionAnObjectAsWork.ToString());

        //if (positionAnObjectAsWork == true)
        //    if (mxc.DesiredDistance - currentDistance > 0.5f)
        //        StopCoroutine(PositionAnObject());
    }

    public IEnumerator PositionAnObject()
    {
        positionAnObjectAsWork = true;

        //currentDistance = mxc.DesiredDistance;

        bool zoomInOrOut = true; // ZoomIn - true; Out - false

        print($"Высота: {Screen.height}, Ширина: {Screen.width}");

        Vector3 centerVector = new Vector3(Screen.width / 2, Screen.height / 2);

        Vector3[] vectorsArray = new Vector3[4]
        {
            new Vector3(0, Screen.height / 2 * sceenPart) + centerVector,
            new Vector3(0, - Screen.height / 2 * sceenPart) + centerVector,
            new Vector3(Screen.width / 2 * sceenPart, 0) + centerVector,
            new Vector3(-Screen.width / 2 * sceenPart, 0)+ centerVector
        };

        foreach(var a in vectorsArray)
        {
            Debug.Log($"Point {a}");
        }

        //mainCamera.ScreenPointToRay();


        // инициализация алгоритма
        for(int i = 0; i < vectorsArray.Length; i++)
        {
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[i])))
                zoomInOrOut = zoomInOrOut && true;
            else
                zoomInOrOut = zoomInOrOut && false;
        }

        Debug.Log($"Приближаем камеру? - {zoomInOrOut}");


        while (true)
        {
            yield return null;


            if(zoomInOrOut == true)
                if (Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[0]))
                 || Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[1]))
                 || Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[2]))
                 || Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[3])))
                    break;
                else
                    mxc.ZoomControl(0.05f);
            else
                if (!Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[0]))
                 && !Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[1]))
                 && !Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[2]))
                 && !Physics.Raycast(Camera.main.ScreenPointToRay(vectorsArray[3])))
                break;
                else
                    mxc.ZoomControl(-0.05f);
        }
        positionAnObjectAsWork = false;
    }
}
