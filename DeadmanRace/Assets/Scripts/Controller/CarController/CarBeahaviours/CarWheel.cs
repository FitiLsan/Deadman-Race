using UnityEngine;
using DeadmanRace.Items;

namespace DeadmanRace.Components
{
    public sealed class CarWheel : BaseCarComponent<Wheel>
    {
        #region Fields

        private WheelCollider _wheel;

        private bool _wheelIsNull = true;

        #endregion


        #region UnityMethods

        private void Awake() => _wheelIsNull = !TryGetComponent(out _wheel);

        #endregion


        #region Methods

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
            if (_descriptionIsNull || _wheelIsNull) return;

            _wheel.brakeTorque = torque;
        }

        #endregion
    }
}