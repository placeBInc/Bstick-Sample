using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Stiffness))]
public class StiffnessButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var button = (Stiffness)target;
        if (GUILayout.Button("ApplyStiffnessEdit"))
        {
            button.ApplyStiffnessEdit();
        }
    }
}
#endif
