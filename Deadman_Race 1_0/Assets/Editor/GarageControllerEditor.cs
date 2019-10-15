using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GarageController))]
public class GarageControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GarageController script = (GarageController)target;
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Load Cars & Tracks") == true)
        {
            script.LoadResources();
        }
        GUILayout.Label(script.EditorLabelText());
    }
}
