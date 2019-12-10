using UnityEngine;
using DeadmanRace.Items;

namespace DeadmanRace.Components
{
    public sealed class CarWheel : BaseCarComponent<Wheel>
    {
        private WheelCollider _wheel;

        private bool _wheelIsNull = true;


        private void Awake() => _wheelIsNull = !TryGetComponent(out _wheel);
        

        public bool GetWheelGroundHit(out WheelHit hit) => _wheel.GetGroundHit(out hit);
        

        public void SetSteerAngle(float value)
        {
            if (_descriptionIsNull || _wheelIsNull) return;

            if (!_description.IsSteerActive) return;

            _wheel.steerAngle = _description.IsMirrorRotation ? -value : value;
        }

        public void SetMotorTorque(float torque)
        {
            if (_descriptionIsNull || _wheelIsNull) return;

            if (!_description.IsDrivable) return;

            _wheel.motorTorque = torque;
        }

        public void SetBreakTorque(float torque)
        {
            if (_wheelIsNull) return;

            _wheel.brakeTorque = torque;
        }
    }
}