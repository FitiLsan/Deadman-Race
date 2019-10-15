using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AIControlSensor))]
public class AIControlSensorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AIControlSensor script = (AIControlSensor)target;
        GUILayout.Label(script.EditorLabelText());
    }
}
