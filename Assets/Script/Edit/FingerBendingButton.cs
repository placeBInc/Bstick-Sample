using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FingerBending))]
public class FingerBendingButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var button = (FingerBending)target;
        if (GUILayout.Button("FingerMapping"))
        {
            button.MappingFinger();
        }
    }
}
