using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalTire : MonoBehaviour
{
    public float LifeTime = 1.0f;
    private float mark = -1;
    private Renderer rend = null;
    private Color color;

    private void Start()
    {
        mark = Time.time;

        rend = this.GetComponent<Renderer>();
        color = rend.material.color;
    }
    private void Update()
    {
        float ElapsedTime = Time.time - mark;
        float PercentTimeLeft = (LifeTime - ElapsedTime) / LifeTime;

        color = rend.material.color;
        color.a = 1.0f - (ElapsedTime / LifeTime);
        rend.material.color = color;

        if (ElapsedTime > LifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
