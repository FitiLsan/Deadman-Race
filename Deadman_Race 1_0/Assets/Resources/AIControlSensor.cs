using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text; //for StringBuilder

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class AIControlSensor : MonoBehaviour
{
    //Note: AI Drive by "sensor only"
    //  pro: simple algorithm
    //  con: not very life like

    #region --- helpers ---
    private struct AIInfo
    {
        public bool sensorFwd;
        public bool sensorLeft;
        public bool sensorRight;
        public RaycastHit hitLeft;
        public RaycastHit hitRight;        
        public Vector3 directionF;
        public Vector3 directionLeft;
        public Vector3 directionRight;
        public string Description()
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.Append(string.Format("sensor fwd on = {0}\n", sensorFwd));
            sb1.Append(string.Format("sensor left = {0}\n", sensorLeft));
            sb1.Append(string.Format("sensor right = {0}\n", sensorRight));            
            return sb1.ToString();
        }
    }
    #endregion

    public float Acceleration = 1;
    public float TurnSpeed = 3;
    public float SensorFwdLen = 7;
    public float SensorSideLen = 5;
    public float Drag = 2;
    public string SoundSensor = "buzz1";
    private AIInfo ai;
    private Rigidbody rb = null;
    private Renderer rend = null;
    private AudioSource sndSensor = null;

    private void Start()
    {
        MeshCollider mcd = this.GetComponent<MeshCollider>();
        mcd.convex = true;

        rb = this.GetComponent<Rigidbody>();
        rb.drag = Drag;

        if (SoundSensor.Length > 0)
        {
            sndSensor = Globals.CreateAudioSource(this.gameObject, SoundSensor);
        }        
    }
    private void FixedUpdate()
    {
        AIControlCalculations();

        //steer
        if (ai.sensorLeft == true)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y - TurnSpeed, this.transform.eulerAngles.z);
            Buzz(true);
        }
        else if (ai.sensorRight == true)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y + TurnSpeed, this.transform.eulerAngles.z);
            Buzz(true);  
        }
        else
        {
            Buzz(false);        
        }

        //accelerate
        if (canAccelerate() == true)
        {
            rb.AddRelativeForce(Vector3.forward * Acceleration, ForceMode.VelocityChange);
        }
    }
    private void OnValidate()
    {
        AIControlCalculations();
    }
    private void OnDrawGizmos()
    {
        // forward sensor
        Gizmos.color = Color.green;
        if (ai.sensorFwd == false)
        {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawRay(centerCar(), ai.directionF * SensorFwdLen);

        // left sensor
        Gizmos.color = Color.white;
        if (ai.sensorRight == true)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(centerCar(), ai.directionLeft * SensorSideLen);

        // right sensor
        Gizmos.color = Color.white;
        if (ai.sensorLeft == true)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(centerCar(), ai.directionRight * SensorSideLen);
    }
    private void AIControlCalculations()
    {
        // directions
        ai.directionF = this.transform.TransformDirection(Vector3.forward);
        ai.directionLeft = (ai.directionF + this.transform.TransformDirection(Vector3.left)).normalized;
        ai.directionRight = (ai.directionF + this.transform.TransformDirection(Vector3.right)).normalized;

        // raycasts
        ai.sensorFwd = !Physics.Raycast(centerCar(), ai.directionF, SensorFwdLen, 1 << Globals.LayerTerrain);
        ai.sensorLeft = Physics.Raycast(centerCar(), ai.directionRight, out ai.hitLeft, SensorSideLen, 1 << Globals.LayerTerrain);
        ai.sensorRight = Physics.Raycast(centerCar(), ai.directionLeft, out ai.hitRight, SensorSideLen, 1 << Globals.LayerTerrain);
    }    
    private bool canAccelerate()
    {
        bool left = (ai.sensorLeft == false || (ai.sensorLeft == true && ai.hitLeft.distance < SensorSideLen / 3.0f));
        if (left == false)
        {
            return false;
        }

        bool right = (ai.sensorRight == false || (ai.sensorRight == true && ai.hitRight.distance < SensorSideLen / 3.0f));
        if (right == false)
        {
            return false;
        } 
        
        if (ai.sensorFwd == false)
        {
            return false;
        }

        return true;
    }
    public Vector3 centerCar()
    {
        if (rend == null)
        {
            rend = this.GetComponent<Renderer>();
        }
        return rend.bounds.center;
    }
    public string EditorLabelText()
    {
        AIControlCalculations();
        return ai.Description();
    }    
    private void Buzz(bool bOn)
    {
        if (sndSensor == null)
        {
            return;
        }
        if (bOn == true && sndSensor.isPlaying == false)
        {
            sndSensor.Play();
        }
        if (bOn == false && sndSensor.isPlaying == true)
        {
            sndSensor.Stop();
        }
    }
}
