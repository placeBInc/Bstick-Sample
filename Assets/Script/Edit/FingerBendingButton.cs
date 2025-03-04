
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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
#endif
