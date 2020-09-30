using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public enum ListType { Command, Mesh };
public class ElementListView : Element<DemonOLPApplication>, ISelectHandler
{
    public int num;
    public ListType ListType { protected set; get; }

    public Text textID;
    public Text textNum;

    protected void Awake()
    {
        int keyID = app.view.commandContainerView.keyCounter++;
        gameObject.name = keyID.ToString();
        textID.text = textID.name + " " + keyID.ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Log(this.gameObject.name + " was selected");

        app.model.focusID = new Vector2Int((int)ListType, int.Parse(gameObject.name));
    }

    public void UpdateNumElement(int num)
    {
        this.num = num;
        textNum.text = textNum.name + " " + num.ToString();
    }


    //// Old version
    //private RectTransform ParentRect
    //{
    //    get { return app.view.commandContainerView.ReorderableList.Content; }
    //}

    //public void UpdateNumElement()
    //{
    //    int contentCount = ParentRect.childCount;

    //    for (int i = 0; contentCount > i; i++)
    //    {
    //        if (gameObject.name == ParentRect.GetChild(i).name)
    //        {
    //            num = i;
    //            textNum.text = textNum.name + " " + i.ToString();
    //            break;
    //        }
    //    }
    //}
}
