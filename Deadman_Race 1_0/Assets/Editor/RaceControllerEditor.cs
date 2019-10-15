using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RaceController))]
public class RaceControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RaceController script = (RaceController)target;
        
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Load Scene") == true)
        {
            script.LoadAll();
        }

        if (GUILayout.Button("Change Track") == true)
        {
            script.ChangeTrack();
        }
        GUILayout.Label("Track: " + script.TrackName());
               
        if (GUILayout.Button("Change Player Car") == true)
        {
            script.ChangePlayerCar();
        }
        GUILayout.Label("Player Car: " + script.PlayerCarName());
    }
}
