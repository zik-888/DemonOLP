using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;

public class DemonOLPController : Controller<DemonOLPApplication>
{
    /// <summary>
    /// Handle notifications from the application.
    /// </summary>
    /// <param name="p_event"></param>
    /// <param name="p_target"></param>
    /// <param name="p_data"></param>
    public override void OnNotification(string p_event, Object p_target, params object[] p_data)
    {
        switch (p_event)
        {
            #region Command
            case "AddCommandButton@click":
                app.view.commandContainerView.Add();
                break;
            case "RemoveCommandButton@click":
                app.view.commandContainerView.Remove(app.model.focusID.y);
                break;
            #endregion

            #region Mesh
            case "AddMeshButton@click":
                app.view.meshContainerView.Add();
                break;
            case "RemoveMeshButton@click":
                app.view.meshContainerView.Remove(app.model.focusID.y);
                break;
            #endregion

            #region ObjectTabSelector
            case "menuTab@change":
                switch (app.view.menuTab.toggle.isOn)
                {
                    case true:
                        app.view.modelTab.toggle.isOn = true;
                        break;
                    case false:
                        app.view.commandTab.toggle.isOn = false;
                        app.view.modelTab.toggle.isOn = false;
                        break;
                }
                break;
            case "commandTab@change":
                switch (app.view.commandTab.toggle.isOn)
                {
                    case true:
                        app.view.commandTab.rectBinding?.gameObject.SetActive(true);
                        break;
                    case false:
                        app.view.commandTab.rectBinding?.gameObject.SetActive(false);
                        break;
                }
                break;
            case "modelTab@change":
                switch (app.view.modelTab.toggle.isOn)
                {
                    case true:
                        app.view.modelTab.rectBinding?.gameObject.SetActive(true);
                        break;
                    case false:
                        app.view.modelTab.rectBinding?.gameObject.SetActive(false);
                        break;
                }
                break;
                #endregion
        }
    }

    private void Update()
    {
    }
}
