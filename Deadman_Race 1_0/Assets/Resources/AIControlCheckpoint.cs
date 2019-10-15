using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text; //for StringBuilder
using System.Linq; //for OrderBy

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class AIControlCheckpoint : MonoBehaviour
{
    //Note: AI Drive by "Checkpoints & sensor" 
    //  pro: life-like
    //  con: race track needs checkpoints to follow

    #region --- helpers ---
    private struct AIInfo
    {
        public int idxTarget;
        public float sideway;
        public float acceleratePercent;
        public float accelerateValue;
        public RaycastHit sideHit;
        public bool sensorF;
        public bool sensorLeft;
        public bool sensorRight;        
        public Vector3 directionF;
        public Vector3 directionLeft;
        public Vector3 directionRight;
        public string Description()
        {
            StringBuilder sb1 = new StringBuilder();
            sb1.Append(string.Format("sensor F = {0}\n", sensorF));
            sb1.Append(string.Format("sensor Left = {0}\n", sensorLeft));
            sb1.Append(string.Format("sensor Right = {0}\n", sensorRight));
            sb1.Append(string.Format("sideway = {0}\n", sideway));
            return sb1.ToString();
        }
    }
    private class PointDistance
    {
        public int idx = -1;
        public float dist = -1;
        public Vector3 directionCP = Vector3.zero;
        public float angFCP = 0; //fwd > car > point
        public bool HasLineOfSight = true;
        public RaycastHit[] hits;
        public override string ToString()
        {
            return string.Format("idx:{0}, dist:{1}, angFCP:{2}, HasLineOfSight:{3}", idx, dist, angFCP, HasLineOfSight);
        }
    }
    #endregion

    public delegate void CarAccelerate(int objectid, float accelerateValue);
    public static event CarAccelerate OnCarAccelerate;

    public float Acceleration = 1;
    public float TurnSpeed = 90.0f;
    public float SensorFwdLen = 7;
    public float SensorSideLen = 5;
    public float Drag = 2;    
    public string SoundSensor = "buzz2";
    public GameObject Checkpoints = null;
    public string WallStartsWith = "Wall";
    public float SideSensorOn = 5.0f;
    private AIInfo ai;
    private Rigidbody rb = null;
    private Renderer rend = null;
    private AudioSource sndSensor = null;
    private int instanceid;
    
    private void Start()
    {
        instanceid = this.gameObject.GetInstanceID();
        rb = this.GetComponent<Rigidbody>();
        rb.drag = Drag;

        GetFirstCheckpoint();
        AIControlCalculations();

        if (SoundSensor.Length > 0)
        {
            sndSensor = Globals.CreateAudioSource(this.gameObject, SoundSensor);
        }
    }
    private void FixedUpdate()
    {
        AIControlCalculations();

        //steer
        Vector3 directionSteer = Checkpoints.transform.GetChild(ai.idxTarget).position - this.transform.position;
        Quaternion rotationSteer = Quaternion.LookRotation(directionSteer);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotationSteer, TurnSpeed * Time.deltaTime);

        //accelerate
        if (canAccelerate() == true)
        {
            ai.accelerateValue = Acceleration * ai.acceleratePercent;
            rb.AddRelativeForce(Vector3.forward * ai.accelerateValue, ForceMode.VelocityChange);
            if (OnCarAccelerate != null)
            {
                OnCarAccelerate(instanceid, ai.accelerateValue);
            }
        }
    }
    private void OnValidate()
    {
        GetFirstCheckpoint();
        AIControlCalculations();
        if (this.Checkpoints == null)
        {
            this.Checkpoints = GameObject.Find(Globals.NameCheckpointHolder);
        }
    }
    private void OnDrawGizmos()
    {
        // forward sensor
        Gizmos.color = Color.green;
        if (ai.sensorF == true)
        {
            Gizmos.color = Color.white;
        }
        Gizmos.DrawRay(centerCar(), ai.directionF * SensorFwdLen);

        if (ai.sideway < 0)
        {
            // left sensor
            Gizmos.color = Color.white;
            if (ai.sensorLeft == true)
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawRay(centerCar(), ai.directionLeft * SensorSideLen);
        }
        else if (ai.sideway > 0)
        {
            // right sensor
            Gizmos.color = Color.white;
            if (ai.sensorRight == true)
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawRay(centerCar(), ai.directionRight * SensorSideLen);
        }

        // target checkpoint
        Gizmos.color = Color.yellow;
        if (Checkpoints != null)
        {
            if (ai.idxTarget <= Checkpoints.transform.childCount - 1)
            {
                Gizmos.DrawLine(this.transform.position, Checkpoints.transform.GetChild(ai.idxTarget).position);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Globals.TagCheckpoint) == true)
        {
            int num = Globals.ExtractNumberValue(other.name) + 1;
            GetNextCheckpoint(num);            
        }
    }
    private void AIControlCalculations()
    {
        //     A     F
        //     .    .
        //     .   .           * F = car forward
        //     .  .            * C = car
        //     . .
        //     . . . . . . B
        //    C 

        //directions
        ai.directionF = this.transform.TransformDirection(Vector3.forward);
        ai.directionLeft = this.transform.TransformDirection(Vector3.left);
        ai.directionRight = this.transform.TransformDirection(Vector3.right);

        //raycast to the front       
        ai.sensorF = Physics.Raycast(centerCar(), ai.directionF, SensorFwdLen, 1 << Globals.LayerTerrain);

        //sideway motion (is car skidding to the side?)
        if (rb == null)
        {
            rb = this.GetComponent<Rigidbody>();
        }
        ai.sideway = this.transform.InverseTransformDirection(rb.velocity).x;

        if (ai.sideway > SideSensorOn)
        {
            //raycast to the left
            ai.sensorLeft = Physics.Raycast(centerCar(), ai.directionRight, out ai.sideHit, SensorSideLen, 1 << Globals.LayerTerrain);
            ai.sensorRight = false;
        }
        else if (ai.sideway < -SideSensorOn)
        {
            //raycast to the right
            ai.sensorLeft = false;
            ai.sensorRight = Physics.Raycast(centerCar(), ai.directionLeft, out ai.sideHit, SensorSideLen, 1 << Globals.LayerTerrain);
        }
    }
    private void GetFirstCheckpoint()
    {
        if (Checkpoints != null && Checkpoints.transform.childCount > 1)
        {
            ai.directionF = this.transform.TransformDirection(Vector3.forward);

            //forward points only
            List<PointDistance> pointdistances = new List<PointDistance>();
            for (int i = 1; i < Checkpoints.transform.childCount; i++)
            {
                if (IsPositionInFront(Checkpoints.transform.GetChild(i).position) == true)
                {
                    PointDistance pd = new PointDistance();
                    pd.idx = i;
                    pd.dist = (this.transform.position - Checkpoints.transform.GetChild(i).position).sqrMagnitude;
                    pd.directionCP = Checkpoints.transform.GetChild(i).position - this.transform.position;
                    pd.angFCP = Vector3.SignedAngle(ai.directionF, pd.directionCP, Vector3.up);
                    pointdistances.Add(pd);
                }
            }
            pointdistances = pointdistances.OrderBy(x => x.dist).ToList();

            //least angle
            List<PointDistance> pds = new List<PointDistance>();
            pds.Add(pointdistances[0]);
            pds.Add(pointdistances[1]);
            pds.Add(pointdistances[2]);
            pds = pds.OrderBy(x => x.angFCP).ToList();
            ai.idxTarget = pds[0].idx;
        }
    }
    private void GetNextCheckpoint(int idxCheckpointHit)
    {
        //1st checkpoint away
        PointDistance pd1 = new PointDistance();
        pd1.idx = NextCheckpointIndex(idxCheckpointHit);
        pd1.dist = (this.transform.position - Checkpoints.transform.GetChild(pd1.idx).position).sqrMagnitude;
        pd1.directionCP = Checkpoints.transform.GetChild(pd1.idx).position - this.transform.position;
        pd1.angFCP = Vector3.SignedAngle(ai.directionF, pd1.directionCP, Vector3.up);
        pd1.hits = Physics.RaycastAll(centerCar(), pd1.directionCP, (Checkpoints.transform.GetChild(pd1.idx).position - this.transform.position).magnitude);
        pd1.HasLineOfSight = (pd1.hits[0].transform.name.StartsWith(WallStartsWith) == true);

        //2nd checkpoint away
        PointDistance pd2 = new PointDistance();
        pd2.idx = NextCheckpointIndex(idxCheckpointHit + 1);
        pd2.dist = (this.transform.position - Checkpoints.transform.GetChild(pd2.idx).position).sqrMagnitude;
        pd2.directionCP = Checkpoints.transform.GetChild(pd2.idx).position - this.transform.position;
        pd2.angFCP = Vector3.SignedAngle(ai.directionF, pd2.directionCP, Vector3.up);
        pd2.hits = Physics.RaycastAll(centerCar(), pd2.directionCP, (Checkpoints.transform.GetChild(pd2.idx).position - this.transform.position).magnitude);
        pd2.HasLineOfSight = (pd2.hits[0].transform.name.StartsWith(WallStartsWith) == true);

        //pick the next
        if (pd2.HasLineOfSight == true)
        {
            ai.idxTarget = pd2.idx;
        }
        else
        {
            ai.idxTarget = pd1.idx;
        }
    }
    private int NextCheckpointIndex(int currentIdx)
    {
        int ret = -1;
        ret = currentIdx + 1;
        if (ret > Checkpoints.transform.childCount - 1)
        {
            ret = (ret - Checkpoints.transform.childCount);
        }
        return ret;
    }
    public bool canAccelerate()
    {
        if (ai.sensorF == true)
        {
            Buzz(true);
            return false;
        }

        if ((ai.sensorLeft == true || ai.sensorRight == true) && Mathf.Abs(ai.sideway) > 3.0f)
        {
            ai.acceleratePercent = Mathf.Clamp(ai.sideway / 9.0f, 0.0f, Acceleration);
            Buzz(true);
            return true;
        }
        else
        {
            ai.acceleratePercent = 1.0f;
            Buzz(false);
            return true;
        }
    }    
    private Vector3 centerCar()
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
        return ai.Description() + string.Format("velocity = {0}\n", rb.velocity.sqrMagnitude);
    }
    private bool IsPositionInFront(Vector3 position)
    {
        return (this.transform.InverseTransformPoint(position).z > 0);
    }
    private void Buzz(bool bOn)
    {
        if (sndSensor != null)
        {
            if (bOn == true && sndSensor.isPlaying == false)
            {
                sndSensor.Play();
            }
            else if (bOn == false && sndSensor.isPlaying == true)
            {
                sndSensor.Stop();
            }
        }
    }
}
