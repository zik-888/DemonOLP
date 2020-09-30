using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;

public class RobotCommandList
{
    private int count;
    public int Count 
    { 
        get { return count; } 
        set 
        {
            if (value < 0)
                count = 0;
            else
                count = value; 
        } 
    }

    public int Focus { set; get; }

}
