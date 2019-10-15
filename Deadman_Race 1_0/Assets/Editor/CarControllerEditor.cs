using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarController))]
public class CarControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.backgroundColor = Color.yellow;
        CarController script = (CarController)target;
        if (GUILayout.Button("Refresh") == true)
        {
            script.UpdateComponents();
        }        
    }
}
