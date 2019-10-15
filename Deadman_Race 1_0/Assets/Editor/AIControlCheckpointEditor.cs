using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIControlCheckpoint))]
public class AIControlCheckpointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AIControlCheckpoint script = (AIControlCheckpoint)target;
        GUILayout.Label(script.EditorLabelText());
    }
}
