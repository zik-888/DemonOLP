using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityScanningActionClient))]
public class UnityScanningActionClientEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Send Goal"))
        {
            ((UnityScanningActionClient)target).SendGoal();
        }

        if (GUILayout.Button("Cancel Goal"))
        {
            ((UnityScanningActionClient)target).CancelGoal();
        }
    }
}
