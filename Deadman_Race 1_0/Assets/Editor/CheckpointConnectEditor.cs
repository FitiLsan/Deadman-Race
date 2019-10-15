using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CheckpointConnect))]
public class CheckpointConnectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        CheckpointConnect script = (CheckpointConnect)target;  
        
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Angle Checkpoint Walls") == true)
        {
            script.AngleCheckpointWalls();
        }

        GUILayout.Label(script.EditorLabelText());
    }
}
