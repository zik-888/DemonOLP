using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeObject : MonoBehaviour
{
    public NodeObject[] Neighbor;

    public AStarPoint Node;



    // Start is called before the first frame update
    void Awake()
    {
        Node = new AStarPoint(transform.position, name);

        Node.initN = GetNPoint;
    }

    private void Update()
    {
        Node.position = transform.position;
    }

    public AStarPoint[] GetNPoint()
    {
        return Neighbor.Select(p => p.Node).ToArray();
    }

    //private void Start()
    //{

    //}

}
