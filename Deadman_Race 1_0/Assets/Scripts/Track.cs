using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public Material trackSkybox = null;

    private void Start()
    {
        SetSkybox();
    }
    private void OnValidate()
    {
        SetSkybox();
    }
    private void SetSkybox()
    {
        RenderSettings.skybox = trackSkybox;
    }
}
