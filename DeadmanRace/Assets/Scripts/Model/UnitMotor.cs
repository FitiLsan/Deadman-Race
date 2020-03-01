using DeadmanRace;
using UnityEngine;

public class UnitMotor : IWalk
{
    private Transform _instance;

    // параметры движения
    private float _maxSpeed = 200;
    private float _curSpeed = 0;
    private float _rotationSpeed = 0.5f;
    private float _accelerationForce = 10;
    private float _brakeForce = 100;
    private float _clatch = 1;

    // входящие параметры направления и движения
    private float _hAxis;
    private float _vAxis;
    private Vector3 _direction;

    private Rigidbody _rgb;


    public UnitMotor(Transform instance, PlayerData playerData)
    {
        _instance = instance;
        _rgb = _instance.gameObject.GetComponent<Rigidbody>();
        _rgb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        _maxSpeed = playerData.MaxSpeed;
        _rotationSpeed = playerData.RotationSpeed;
        _accelerationForce = playerData.AccelerationForce;
        _brakeForce = playerData.BrakeForce;
        _clatch = playerData.Clatch;
    }

    public Transform Transform { get; set; }

    public void Walk(float hAxis, float vAxis)
    {
        CharacterRotation(hAxis);
        CharacterMove(hAxis, vAxis);
    }

    private void CharacterMove(float hAxis, float vAxis)
    {
        if (vAxis > 0)
            _curSpeed += _accelerationForce * vAxis;
        else
            _curSpeed += -1 * (_brakeForce * 0.5f - _clatch) + (_brakeForce - _clatch) * vAxis;
        //CustomDebug.Log($"Iteration1: CurSpeed = {_curSpeed}");
        if (_curSpeed > _maxSpeed * 0.98f) _curSpeed = _maxSpeed;
        if (_curSpeed < 0.05 * _maxSpeed) _curSpeed = 0;
        //CustomDebug.Log($"Iteration2: CurSpeed = {_curSpeed}");
        _rgb.AddForce(_instance.transform.forward * _curSpeed);
    }
    private void CharacterRotation(float hAxis)
    {
        if (hAxis != 0)
        {
            if (_curSpeed < 0.1 * _maxSpeed)
                _rgb.AddForce(_instance.transform.forward * _rgb.mass * 6f);
            _instance.rotation *= Quaternion.Euler(0, hAxis * _rotationSpeed, 0);
            _instance.gameObject.transform.rotation = _instance.rotation;
        }
    }
}
