using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class TabObjectView : ToggleView
{
    public RectTransform rectBinding;

    private new void Awake()
    {
        base.Awake();

        if (rectBinding != null)
            switch (toggle.isOn)
            {
                case true:
                    rectBinding.gameObject.SetActive(true);
                    break;
                case false:
                    rectBinding.gameObject.SetActive(false);
                    break;
            }
        else
            Notify(notification + "@change");
    }

    
}
