using UnityEngine.UI.Extensions;
using System.Collections;
using thelab.mvc;
using UnityEngine;

public class ListContainerView : NotificationView<DemonOLPApplication>
{

#pragma warning disable 0649
    [SerializeField] protected GameObject elementListPrefab;
    [SerializeField] protected ReorderableList reorderableList;
#pragma warning restore 0649

    public ReorderableList ReorderableList
    {
        get { return reorderableList; }
    }

    public int keyCounter = 0;

    public int Count
    {
        get { return reorderableList.Content.childCount; }
    }

    public void Add()
    {
        Instantiate(elementListPrefab, reorderableList.Content.gameObject.transform);
        ContainerUpdate();
    }

    public void Remove(int elementID = -1)
    {
        if (reorderableList.Content.childCount != 0)
        {
            if (elementID == -1)
            {
                elementID = reorderableList.Content.childCount - 1;
                Destroy(reorderableList.Content.GetChild(elementID).gameObject);
            }
            else
            {
                Log(elementID.ToString());
                Destroy(reorderableList.Content.Find(elementID.ToString()).gameObject);
                app.model.focusID = -Vector2Int.one;
            }

            ContainerUpdate();
        }
    }

    public void ContainerUpdate() => StartCoroutine(ContainerUpdateInNextFrame());

    virtual protected IEnumerator ContainerUpdateInNextFrame()
    {
        yield return null;
    }
}
