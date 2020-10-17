using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SFB;
using System;

using Microsoft.Win32;

public class ObjImportFile : MonoBehaviour
{
    public InputField inputField;
    //public string objPath = @"Assets/Model DataBase/Test.obj";
    protected string objPath = @"Assets/Model DataBase/cube/untitled.obj";
    public GameObject parentObject;

    public GameObject testObject;

    public Material material;

    public void ChangePath(string path)
    {
        objPath = @path;
    }

    public void Open()
    {
        string[] path = StandaloneFileBrowser.OpenFilePanel("Overwrite with obj", @"Assets\Model DataBase", "obj", false);


        //("Select Folder", "", true);
        print(path[0]);

        if (path.Length != 0)
        {
            objPath = @path[0];
            inputField.text = objPath;
        }

    }


    public GameObject Load(string name)
    {
        if (!File.Exists(objPath))
        {
            Debug.LogError("Please set FilePath in ObjFromFile.cs to a valid path.");

            GameLog.Log("Please set FilePath in ObjFromFile.cs to a valid path.");
            return null;
        }
        else
        {
            try
            {
                GameLog.Log(objPath);
                var loadedOBJ = new OBJLoader().Load(objPath);
                GameLog.Log(loadedOBJ.name);

                var loadedObject = loadedOBJ.transform.GetChild(0).gameObject;
                loadedObject.transform.parent = parentObject.transform;
                loadedObject.transform.localScale = Vector3.one;
                loadedObject.AddComponent<MeshCollider>();
                loadedObject.GetComponent<MeshRenderer>().material = material;
                loadedObject.name = name;
                return loadedObject;
            }
            catch(Exception e)
            {
                GameLog.Log(e.Message);
                return null;
            }
        }
    }
}
