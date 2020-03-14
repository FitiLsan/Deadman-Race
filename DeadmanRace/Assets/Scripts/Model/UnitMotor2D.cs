using DeadmanRace;
using System;
using UnityEngine;

public sealed class UnitMotor2D : IWalk
{
    private Transform _instance;

    #region Moving parameters

    private readonly float _maxSpeed = 200.0f;
    private readonly float _rotationSpeed = 0.5f;
    private readonly float _accelerationForce = 10.0f;
    private readonly float _brakeForce = 100.0f;
    private readonly float _clatch = 1.0f;
    private float _curSpeed = 0.0f;

    #endregion

    private Rigidbody2D _rigidBody;


    public UnitMotor2D(Transform instance, PlayerData playerData)
    {
        _instance = instance;
        _rigidBody = _instance.gameObject.GetComponent<Rigidbody2D>();
        _maxSpeed = playerData.MaxSpeed;
        _rotationSpeed = playerData.RotationSpeed;
        _accelerationForce = playerData.AccelerationForce;
        _brakeForce = playerData.BrakeForce;
        _clatch = playerData.Clatch;
    }

    public Transform Transform { get; set; }

    public void Walk(float horizontalAxis, float verticalAxis)
    {
        CharacterRotation(horizontalAxis);
        CharacterMove(horizontalAxis, verticalAxis);
    }

    private void CharacterMove(float horizontalAxis, float verticalAxis)
    {
        if (verticalAxis > 0.1f)
            _curSpeed += _accelerationForce * verticalAxis;
        else
        {
            CustomDebug.Log($"Iteration1: CurSpeed = {_curSpeed}");
            _curSpeed += -1 * (_brakeForce * 0.5f / _clatch) + (_brakeForce / _clatch) * verticalAxis;
        CustomDebug.Log($"Iteration2: vAxis = {verticalAxis}, CurSpeed = {_curSpeed}, Brake speed = {-1 * (_brakeForce * 0.5f / _clatch) + (_brakeForce / _clatch) * verticalAxis}");
            // CustomDebug.Log($"Iteration2: CurSpeed = {_curSpeed}");
        }
        if (_curSpeed > _maxSpeed * 0.98f) _curSpeed = _maxSpeed;
        else if (_curSpeed < -1 * _maxSpeed * 0.98f) _curSpeed = -1 * _maxSpeed;
        else if (Math.Abs(_curSpeed) < 0.05f * _maxSpeed) _curSpeed = 0.0f;
        // CustomDebug.Log($"Iteration2: CurSpeed = {_curSpeed}");
        _rigidBody.AddForce(_instance.transform.up * _curSpeed);
    }
    private void CharacterRotation(float horizontalAxis)
    {
        if (horizontalAxis != 0.0f)
        {
            if (_curSpeed < 0.1f * _maxSpeed)
                _rigidBody.AddForce(_instance.transform.up * _rigidBody.mass * 6.0f);
            _instance.rotation *= Quaternion.Euler(0.0f, 0.0f, horizontalAxis * _rotationSpeed);
            _instance.gameObject.transform.rotation = _instance.rotation;
        }
    }
}
