using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Stiffness))]
public class StiffnessButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var button = (Stiffness)target;
        if (GUILayout.Button("ApplyStiffness"))
        {
            button.ApplyStiffness();
        }
    }
}
