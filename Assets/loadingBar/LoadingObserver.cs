using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;
using UniRx;

public class LoadingObserver : Element<DemonOLPApplication>
{
    public GameObject loadAnim;

    // Start is called before the first frame update
    void Start()
    {


        app.model.LoadedAmination
                 .ObserveEveryValueChanged(x => x.Value)
                 .Subscribe(xs => loadAnim.SetActive(xs
                 ))
                 .AddTo(this);
    }


}
