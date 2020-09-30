using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;

public class DemonOLPView : View<DemonOLPApplication>
{
    public TabObjectView menuTab;
    public TabObjectView commandTab;
    public TabObjectView modelTab;


    public CommandContainerView commandContainerView;
    public MeshContainerView meshContainerView;
}
