using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for Text

public class CarSounds : MonoBehaviour
{ 
    public string SoundEngine = "Engine02";
    public string SoundSkid = "Skid";
    public string SoundCrash = "HitCarToCar";
    public float SkidStart = 3.0f;
    public float RumbleVolume = 0.1f;
    public float EnginePitchSensitivity = 0.80f;
    private AudioSource sndEngine = null;
    private AudioSource sndRumble = null;
    private AudioSource sndSkid = null;
    private AudioSource sndCrash = null;
    private Rigidbody rb = null;
    private int instanceid;

    private void OnEnable()
    {
        BumperCollider.OnBumperColliderHit += OnBumperColliderHit;
        CarControl.OnCarAccelerate += OnCarAccelerate;
        AIControlCheckpoint.OnCarAccelerate += OnAICarAccelerate;
    }
    private void OnDisable()
    {
        BumperCollider.OnBumperColliderHit -= OnBumperColliderHit;
        CarControl.OnCarAccelerate -= OnCarAccelerate;
        AIControlCheckpoint.OnCarAccelerate -= OnAICarAccelerate;
    }
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        instanceid = this.gameObject.GetInstanceID();

        //engine
        sndEngine = Globals.CreateAudioSource(this.gameObject, SoundEngine);
        sndEngine.volume = 1.0f;
        sndEngine.loop = true;
        sndRumble = Globals.CreateAudioSource(this.gameObject, SoundEngine);
        sndRumble.volume = RumbleVolume;
        sndRumble.loop = true;
        sndRumble.Play();

        //skid
        sndSkid = Globals.CreateAudioSource(this.gameObject, SoundSkid);
        sndSkid.loop = true;

        //crash
        sndCrash = Globals.CreateAudioSource(this.gameObject, SoundCrash);
    }
    private void FixedUpdate()
    {
        //sideway motion of car relative to itself (not world)
        float sideway = Mathf.Abs(this.transform.InverseTransformDirection(rb.velocity).x);
        if (sideway > SkidStart)
        {
            //play skid sound
            if (sndSkid.isPlaying == false)
            {
                sndSkid.Play();
            }            
            sndSkid.volume = 0.1f + (sideway / 30.0f);
            sndSkid.pitch = 1.0f + (sideway / 60.0f);
        }
        else
        {
            sndSkid.Stop();
        }
    }
    private void OnBumperColliderHit(int objectid, Collider other)
    {
        if (objectid == instanceid)
        {
            sndCrash.Play();
        }
    }
    private void OnCarAccelerate(int objectid, float acceleratevalue)
    {
        if (objectid == instanceid)
        {
            if (Mathf.Abs(acceleratevalue) > 0.1f)
            {
                if (sndEngine.isPlaying == false)
                {
                    sndEngine.Play();
                }
                sndEngine.pitch = 1.0f + (acceleratevalue * EnginePitchSensitivity);
            }
            else
            {
                sndEngine.Stop();
            }
        }
    }
    private void OnAICarAccelerate(int objectid, float acceleratevalue)
    {
        if (objectid == instanceid)
        {
            sndRumble.Stop();
            sndEngine.volume = 0.07f;
            if (Mathf.Abs(acceleratevalue) > 0.1f)
            {
                if (sndEngine.isPlaying == false)
                {
                    sndEngine.Play();
                }
                sndEngine.pitch = 1.0f + (acceleratevalue * EnginePitchSensitivity);
            }
            else
            {
                sndEngine.Stop();
            }
        }
    }
}
