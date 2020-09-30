using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarCalc : MonoBehaviour
{
    public NodeObject[] nodeObjects;

    //public int numStartNode = 0;
    //public int numGoalNode = 2;
    enum pointName { A, B, C, D }

    private void Start()
    {
        nodeObjects = GetComponentsInChildren<NodeObject>();

        //var pA = nodeObjects[(int)pointName.A];
        //var pB = nodeObjects[(int)pointName.B];
        //var pC = nodeObjects[(int)pointName.C];
        //var pD = nodeObjects[(int)pointName.D];


        var path = AStarPathfinder.AStarCalc(nodeObjects.First(p => p.name == "A").Node,
                                             nodeObjects.First(p => p.name == "C").Node);

        foreach(var a in path)
        {
            Debug.Log(a.ToString());
        }

        Debug.DrawLine(path[0].position, path[1].position, Color.black);
        Debug.DrawLine(path[1].position, path[2].position, Color.black);

        StartCoroutine(UPD());
    }

    private IEnumerator UPD()
    {
        

        while (true)
        {
            yield return /*new WaitForSeconds(0.1f)*/ null;


            var path = AStarPathfinder.AStarCalc(nodeObjects.First(p => p.name == "A").Node,
                                             nodeObjects.First(p => p.name == "C").Node);


            Debug.DrawLine(path[0].position, path[1].position, Color.black);
            Debug.DrawLine(path[1].position, path[2].position, Color.black);
        }
    }
}
