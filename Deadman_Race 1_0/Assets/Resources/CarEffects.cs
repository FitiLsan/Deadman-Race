using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    public float TireMarkIntensity = 0.02f;
    public GameObject DecalTirePrefab = null;
    public GameObject PrefabHolder = null;
    private int instanceid;
    private float decalSpawnInterval = 0.01f;
    private float mark = -1;

    private void OnEnable()
    {
        CarControl.OnCarAccelerate += OnCarAccelerate;
        AIControlCheckpoint.OnCarAccelerate += OnCarAccelerate;
    }
    private void OnDisable()
    {
        CarControl.OnCarAccelerate -= OnCarAccelerate;
        AIControlCheckpoint.OnCarAccelerate -= OnCarAccelerate;
    }
    private void Start()
    {
        instanceid = this.gameObject.GetInstanceID();
       
        if (DecalTirePrefab == null)
        {
            DecalTirePrefab = Globals.LoadPrefab(Globals.NameDecalTireDirt);
        }
        if (PrefabHolder == null)
        {
            PrefabHolder = GameObject.Find(Globals.NamePrefabHolder);
        }               
    }    
    private void SpawnDecal()
    {
        GameObject decal = Instantiate(DecalTirePrefab, PrefabHolder.transform);
        decal.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        decal.transform.Rotate(Vector3.back, this.transform.eulerAngles.y);
        mark = Time.time;
    }
    private void OnCarAccelerate(int objectid, float acceleratevalue)
    {
        if (objectid == instanceid)
        {
            if (acceleratevalue > 0.1f && (Time.time - mark > decalSpawnInterval || mark == -1))
            {
                SpawnDecal();
            }
        }
    }
}
