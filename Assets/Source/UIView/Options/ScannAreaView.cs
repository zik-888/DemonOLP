using System.Collections;
using System.Collections.Generic;
using thelab.mvc;
using UnityEngine;
using UnityEngine.UI;

public class ScannAreaView : Element<DemonOLPApplication>
{
    public InputField x1;
    public InputField y1;
    public InputField x2;
    public InputField y2;

    // Start is called before the first frame update
    void Start()
    {
        x1.text = app.model.scannArea.x.ToString();
        y1.text = app.model.scannArea.y.ToString();
        x2.text = app.model.scannArea.z.ToString();
        y2.text = app.model.scannArea.w.ToString();
    }

    public void ChangeAreaView()
    {
        app.model.scannArea.x = float.Parse(x1.text);
        app.model.scannArea.y = float.Parse(y1.text);
        app.model.scannArea.z = float.Parse(x2.text);
        app.model.scannArea.w = float.Parse(y2.text);
    }
}
