using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIManager), true)]
public class TestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Vector3 vec = Vector3.one;
        GUILayout.Label("Mon label");
        EditorGUILayout.Vector3Field("Vector", vec);
    }
}