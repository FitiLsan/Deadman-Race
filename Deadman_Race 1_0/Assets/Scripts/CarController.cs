using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //for Exception

//Note: configurable resources
//  scripts (5):            prefabs (5):    
//  ------------            ------------
//  CarControl              DecalTireDirt
//  CarEffects              DecalTirePavement
//  CarSounds               FollowPos
//  AIControlCheckpoint     EffectDustTrail
//  AIControlSensor         Bumper
/// <summary>
/// все типы AI
/// </summary>
public class CarController : MonoBehaviour
{
    #region --- helpers ---
    public enum enumCarType
    {
        Undefined,
        Player,
        AIControlCheckpoint,
        AIControlSensor,
    }
    #endregion

    public enumCarType CarType = enumCarType.Undefined;
    
    public void UpdateComponents()
    {
        switch (CarType)
        {
            case enumCarType.Undefined:
                hlprScriptUndefined();
                break;
            case enumCarType.Player:
                hlprScriptPlayer();
                break;
            case enumCarType.AIControlCheckpoint:
                hlprScriptAIControlCheckpoint();
                break;
            case enumCarType.AIControlSensor:
                hlprScriptAIControlSensor();
                break;
        }
    }
    /// <summary>
    /// Неопределенный тип AI - удаляет контроллеры управления, следования до чекпоинтов и сенсоры
    /// </summary>
    private void hlprScriptUndefined()
    {
        //scripts (5)
        if (this.GetComponent<CarControl>() != null)
        {
            DestroyImmediate(this.GetComponent<CarControl>());
        }
        if (this.GetComponent<AIControlCheckpoint>() != null)
        {
            DestroyImmediate(this.GetComponent<AIControlCheckpoint>());
        }
        if (this.GetComponent<AIControlSensor>() != null)
        {
            DestroyImmediate(this.GetComponent<AIControlSensor>());
        }
        if (this.GetComponent<CarEffects>() != null)
        {
            DestroyImmediate(this.GetComponent<CarEffects>());
        }      
        if (this.GetComponent<CarSounds>() != null)
        {
            DestroyImmediate(this.GetComponent<CarSounds>());
        } 
        
        //other (5)
        if (this.GetComponent<Rigidbody>() != null)
        {
            DestroyImmediate(this.GetComponent<Rigidbody>());
        }
        if (this.GetComponent<MeshCollider>() != null)
        {
            DestroyImmediate(this.GetComponent<MeshCollider>());
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "FollowPos")
            {
                DestroyImmediate(this.transform.GetChild(i).gameObject);
                break;
            }
        }
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "EffectDustTrail")
            {
                DestroyImmediate(this.transform.GetChild(i).gameObject);
                break;
            }
        }
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "Bumper")
            {
                DestroyImmediate(this.transform.GetChild(i).gameObject);
                break;
            }
        }
    }
    /// <summary>
    /// Тип AI - игрок. Есть возможность управления машиной CarControl, добавляет декали эфекты и звуки
    /// </summary>
    private void hlprScriptPlayer()
    {
        hlprScriptUndefined();

        CarControl carcontrol = null;
        
        //script (3)
        if (this.gameObject.GetComponent<CarControl>() == null)
        {
            carcontrol = this.gameObject.AddComponent<CarControl>();
        }
        if (this.gameObject.GetComponent<CarEffects>() == null)
        {
            this.gameObject.AddComponent<CarEffects>();
        }
        if (this.gameObject.GetComponent<CarSounds>() == null)
        {
            this.gameObject.AddComponent<CarSounds>();
        }

        //other (5)
        AddRigidBody(carcontrol.Drag);
        AddMeshCollider();
        AddFollowPos();
        AddEffectDust();
        AddBumper();
    }
    /// <summary>
    /// AI противника езды по чекпоинтам
    /// </summary>
    private void hlprScriptAIControlCheckpoint()
    {
        hlprScriptUndefined();

        AIControlCheckpoint aicontrolcheckpoint = null;
        
        //script (3)
        if (this.gameObject.GetComponent<AIControlCheckpoint>() == null)
        {
            aicontrolcheckpoint = this.gameObject.AddComponent<AIControlCheckpoint>();
        }
        if (this.gameObject.GetComponent<CarEffects>() == null)
        {
            this.gameObject.AddComponent<CarEffects>();
        }
        if (this.gameObject.GetComponent<CarSounds>() == null)
        {
            this.gameObject.AddComponent<CarSounds>();
        }

        //other (5)
        AddRigidBody(aicontrolcheckpoint.Drag);
        AddMeshCollider();
        AddFollowPos();
        AddEffectDust();
        AddBumper();
    }
    /// <summary>
    /// AI противника езды с помощью сенсоров, при столкновении с бампером
    /// </summary>
    private void hlprScriptAIControlSensor()
    {
        hlprScriptUndefined();

        AIControlSensor aicontrolsensor = null;

        //script (2)
        if (this.gameObject.GetComponent<AIControlSensor>() == null)
        {
           aicontrolsensor = this.gameObject.AddComponent<AIControlSensor>();
        }
        if (this.gameObject.GetComponent<CarEffects>() == null)
        {
            this.gameObject.AddComponent<CarEffects>();
        }
        if (this.gameObject.GetComponent<CarSounds>() == null)
        {
            this.gameObject.AddComponent<CarSounds>();
        }

        //other (5)
        AddRigidBody(aicontrolsensor.Drag);
        AddMeshCollider();
        AddFollowPos();
        AddEffectDust();
        AddBumper();
    }
    public void AddRigidBody(float drag)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = this.gameObject.AddComponent<Rigidbody>();
        }
        rb.drag = drag;
    }
    public void AddMeshCollider()
    {
        MeshCollider mcd = this.GetComponent<MeshCollider>();
        if (mcd == null)
        {
            mcd = this.gameObject.AddComponent<MeshCollider>();
        }
        mcd.convex = true;
    }
    public void AddFollowPos()
    {
        //clear
        List<GameObject> array = new List<GameObject>();
        foreach(Transform child in this.transform)
        {
            if (child.name.StartsWith(Globals.NameFollowPos) == true)
            {
                array.Add(child.gameObject);
            }
        }
        foreach (GameObject obj in array)
        {
            DestroyImmediate(obj);
        }

        //add follow positions (multiple camera follow positions)        
        GameObject prefab = Resources.Load("FollowPos") as GameObject;
        GameObject followpos = Instantiate(prefab, this.transform);
        followpos.name = "FollowPos";
    }
    public void AddEffectDust()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "EffectDustTrail")
            {
                return;
            }
        }
        GameObject prefab = Resources.Load("EffectDustTrail") as GameObject;
        GameObject effectdust = Instantiate(prefab, this.transform);
        effectdust.name = "EffectDustTrail";
    }
    /// <summary>
    /// добавляет префаб из ресурсов "Bumper" 
    /// </summary>
    public void AddBumper()
    {
        //exists?
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "Bumper")
            {
                return;
            }
        }
        //load bumper and instantiate
        GameObject prefab = Resources.Load("Bumper") as GameObject;
        GameObject bumper = Instantiate(prefab, this.transform);
        bumper.name = "Bumper";
        //size bumper to car
        BumperCollider script = bumper.GetComponent<BumperCollider>();
        script.AutoSizePositionBumper();
    }
}
