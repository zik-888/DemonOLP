
using UnityEngine;
using UnityEngine.EventSystems;

public class CommandView : ElementListView
{
    public RobotCommand RC;


    //protected RectTransform parentObject;


    

    private new void Awake()
    {
        
        //Instantiate(parentObject, app.view.meshContainerView.importer.parentObject.transform);

        int keyID = app.view.commandContainerView.keyCounter++;
        gameObject.name = keyID.ToString();
        textID.text = textID.name + " " + keyID.ToString();
        ListType = ListType.Command;
        RC = new RobotCommand();


        var parentObject = new GameObject($"{nameof(CommandView)}: {name}");
        parentObject.transform.parent = app.view.meshContainerView.importer.parentObject.transform;
        gameObject.AddComponent<PickMarching>().parentObject = parentObject;
    }

    public void SetPointCommand()
    {

    }


}
