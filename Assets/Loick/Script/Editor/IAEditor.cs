using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using  UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(AIController))]
public class AIEditor : Editor
{
     AIController aIControllerEditor;
     private void OnEnable()
    {
        aIControllerEditor = target as AIController;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Label("Current Delay " + aIControllerEditor.GetDelay());
    }
}
