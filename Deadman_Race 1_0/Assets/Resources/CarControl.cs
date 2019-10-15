using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //for Text

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CarControl : MonoBehaviour
{
    #region --- helpers ---
    [System.Serializable]
    public enum enumControlScheme
    {
        Keyboard,
        Joystick,
    }
    #endregion

    public delegate void CarAccelerate(int objectid, float accelerateValue);
    public static event CarAccelerate OnCarAccelerate;

    public float Acceleration = 1.0f;
    public float TurnSpeed = 90.0f;
    public float Drag = 2;
    public enumControlScheme ControlScheme;
    private Rigidbody rb = null;
    private int instanceid;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.drag = Drag;
        instanceid = this.gameObject.GetInstanceID();
    }
    private void FixedUpdate()
    {
        switch (ControlScheme)
        {
            case enumControlScheme.Keyboard:
                KeyboardControl();
                break;
            case enumControlScheme.Joystick:
                JoystickControl();
                break;
        }
    }
    private void KeyboardControl()
    {
        //steer
        float X = Input.GetAxis("Horizontal");
        this.transform.rotation *= Quaternion.AngleAxis(TurnSpeed * X * Time.deltaTime, Vector3.up);

        //accelerate 
        float Z = Input.GetAxis("Vertical");
        if (Z != 0)
        {
            rb.AddRelativeForce(Vector3.forward * Z * Acceleration, ForceMode.VelocityChange);
        }
        if (OnCarAccelerate != null)
        {
            OnCarAccelerate(instanceid, Z);
        }
    }
    private void JoystickControl()
    {
        //Note: InputManager.asset (for the joystick control definitions)

        //steer
        float X = Input.GetAxis("LeftAnalogXAll");
        this.transform.rotation *= Quaternion.AngleAxis(TurnSpeed * X * Time.deltaTime, Vector3.up);

        //accelerate 
        float Z = -Input.GetAxis("RTAll");
        if (Z != 0)
        {
            rb.AddRelativeForce(Vector3.forward * Z * Acceleration, ForceMode.VelocityChange);
        }
        if (OnCarAccelerate != null)
        {
            OnCarAccelerate(instanceid, Z);
        }
    }
}
