using MeshSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeshView : ElementListView, ISelectHandler
{
    protected MeshInitializer meshInitializer { set; get; }
    protected CamMoveControl cmc;

    private new void Awake()
    {
        int keyID = app.view.meshContainerView.keyCounter++;
        gameObject.name = keyID.ToString();
        app.model.meshModelNameArray.Add(gameObject.name);
        textID.text = textID.name + " " + keyID.ToString();
        ListType = ListType.Mesh;
        meshInitializer = app.view.meshContainerView.importer.Load(keyID.ToString()).AddComponent<MeshInitializer>();

        ////////////////////////////////////////////////////////////
        
        cmc = gameObject.AddComponent<CamMoveControl>();
        StartCoroutine(cmc.PositionAnObject());
    }

    public new void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        StartCoroutine(cmc.PositionAnObject());
    }

    protected void OnDestroy()
    {
        Destroy(meshInitializer.gameObject);
    }

}
