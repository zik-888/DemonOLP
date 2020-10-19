using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshContainerView : ListContainerView
{
    public ObjImportFile importer;
    override protected IEnumerator ContainerUpdateInNextFrame()
    {
        yield return new WaitForEndOfFrame();

        int contentCount = reorderableList.Content.childCount;

        for (int i = 0; contentCount > i; i++)
        {
            reorderableList.Content.GetChild(i).GetComponent<MeshView>().UpdateNumElement(i);
        }
    }

}
