using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;
using UniRx;

public enum PathFormMethod { PointToPoint, PickMarching, DrawProjection };

public class DemonOLPModel : Model<DemonOLPApplication>
{
    public bool loadedAmination = false;
    public ReactiveProperty<bool> LoadedAmination { set; get; } = new ReactiveProperty<bool>(false);

    public bool IsLoadScannModel { set; get; } = false;
    public Mesh CurrentLoadScannModel { set; get; }
    public Material baseMaterial;
    public PathFormMethod pathFormMethod = PathFormMethod.PointToPoint;
    public List<ITrajectory> Trajectories { set; get; } = new List<ITrajectory>();

    public Vector3 deltaModel;

    public Vector4 scannArea;
    /// <summary>
    /// Programm data.
    /// </summary>
    public RobotCommand[] commandArray;

    /// <summary>
    /// ID focus re-orderable list: 
    /// 1 element - list number; 
    /// 2 element - element in list number
    /// -1 = NuN
    /// </summary>
    public Vector2Int focusID = - Vector2Int.one;

    public List<string> meshModelNameArray { set; get; } = new List<string>();

    [SerializeField]
    public GameObject planeDNG;
    [SerializeField]
    public GameObject vertexPoint;
    [SerializeField]
    public GameObject refPoint;



}
