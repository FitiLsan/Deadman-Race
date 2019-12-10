using UnityEngine;
using DeadmanRace.Items;

namespace DeadmanRace.Components
{
    public sealed class MotionModel
    {
        private const int EVERY_N_FRAME = 5;

        private CarTemplate _model;
        private bool _modelIsNull = true;

        private Transform _transform;

        private Rigidbody _rigidbody;

        private CarCarcase _carcase;
        private CarEngine _engine;
        private CarFuelTank _fuelTank;
        private CarWheel[] _wheels;

        #region StandartAssetCarControllerPrivateVariables

        private const float m_MaxHandbrakeTorque = float.MaxValue;
        private const float k_ReversingThreshold = 0.01f;

        private float m_SteerHelper; // 0 is raw physics , 1 the car will grip in the direction it is facing
        private float m_MaximumSteerAngle;
        private float m_FullTorqueOverAllWheels;
        private float m_TractionControl; // 0 is no traction control, 1 is full interference
        private Vector3 m_CentreOfMassOffset;
        private float m_ReverseTorque;
        private float m_Downforce;
        private float m_Topspeed;
        private int NoOfGears;
        private float m_RevRangeBoundary;
        private float m_SlipLimit;
        private float m_BrakeTorque;

        private Quaternion[] m_WheelMeshLocalRotations;
        private Vector3 m_Prevpos, m_Pos;
        private float m_SteerAngle;
        private int m_GearNum;
        private float m_GearFactor;
        private float m_OldRotation;
        private float m_CurrentTorque;

        #endregion

        #region StandartAssetCarControllerProperties

        public bool Skidding { get; private set; }
        public float BrakeInput { get; private set; }
        public float CurrentSteerAngle { get { return m_SteerAngle; } }
        public float CurrentSpeed { get { return _rigidbody.velocity.magnitude * 2.23693629f; } }
        public float MaxSpeed { get { return m_Topspeed; } }
        public float Revs { get; private set; }
        public float AccelInput { get; private set; }

        #endregion

        public MotionModel(Transform car, CarTemplate model)
        {
            _model = model ?? throw new System.NullReferenceException("[CarController] CarTemplate is null");
            _modelIsNull = false;

            _transform = car;

            _rigidbody = _transform.GetComponentInChildren<Rigidbody>();

            _carcase = _transform.GetComponentInChildren<CarCarcase>();

            _engine = _transform.GetComponentInChildren<CarEngine>();

            _fuelTank = _transform.GetComponentInChildren<CarFuelTank>();

            _wheels = _transform.GetComponentsInChildren<CarWheel>();

            InitializeStandartAssetVariables();
        }

        private void InitializeStandartAssetVariables()
        {
            m_FullTorqueOverAllWheels = _engine.GetTorque(_fuelTank, Time.deltaTime * EVERY_N_FRAME * _rigidbody.velocity.magnitude);
            m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);
            m_SteerHelper = _model.GetSteerHelper;
            m_MaximumSteerAngle = _model.GetMaximumSteerAngle;
            m_TractionControl = _model.GetTractionControl;
            m_CentreOfMassOffset = _model.GetCentreOfMassOffset;
            m_ReverseTorque = _engine.GetTorque(_fuelTank, Time.deltaTime * EVERY_N_FRAME * _rigidbody.velocity.magnitude) / _model.GetNoOfGears;
            m_Downforce = _model.GetDownforce;
            m_Topspeed = _model.GetTopspeed;
            NoOfGears = _model.GetNoOfGears;
            m_RevRangeBoundary = _model.GetRevRangeBoundary;
            m_SlipLimit = _model.GetSlipLimit;
            m_BrakeTorque = _model.GetBrakeTorque;
        }
        
        public void Move(float steering, float accel, float footbrake, float handbrake)
        {
            if (_modelIsNull) return;

            //clamp input values
            if (Time.frameCount % EVERY_N_FRAME == 0)
            {
                m_FullTorqueOverAllWheels = _engine.GetTorque(_fuelTank, Time.deltaTime * EVERY_N_FRAME * _rigidbody.velocity.magnitude);
                m_ReverseTorque = m_FullTorqueOverAllWheels / NoOfGears;
            }

            steering = Mathf.Clamp(steering, -1, 1);
            AccelInput = accel = Mathf.Clamp(accel, 0, 1);
            BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
            handbrake = Mathf.Clamp(handbrake, 0, 1);

            //Set the steer on the front wheels.
            //Assuming that wheels 0 and 1 are the front wheels.
            m_SteerAngle = steering * m_MaximumSteerAngle;

            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i].SetSteerAngle(m_SteerAngle);
            }

            SteerHelper();
            TractionControl();
            ApplyDrive(accel, footbrake);
            CapSpeed();

            //Set the handbrake.
            var hbTorque = handbrake * m_MaxHandbrakeTorque;

            if (handbrake > 0f)
            {
                for (int i = 0; i < 2; i++)
                {
                    _wheels[_wheels.Length - i - 1].SetBreakTorque(hbTorque);
                }
            }


            CalculateRevs();
            GearChanging();
            AddDownForce();
            CheckForWheelSpin();
        }
        

        #region StandartAssetCarController

        private void GearChanging()
        {
            float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
            float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
            float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

            if (m_GearNum > 0 && f < downgearlimit)
            {
                m_GearNum--;
            }

            if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
            {
                m_GearNum++;
            }
        }

        // simple function to add a curved bias towards 1 for a value in the 0-1 range
        private static float CurveFactor(float factor) => 1 - (1 - factor) * (1 - factor);

        // unclamped version of Lerp, to allow value to exceed the from-to range
        private static float ULerp(float from, float to, float value) => (1.0f - value) * from + value * to;

        private void CalculateGearFactor()
        {
            float f = (1 / (float)NoOfGears);
            // gear factor is a normalised representation of the current speed within the current gear's range of speeds.
            // We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
            var targetGearFactor = Mathf.InverseLerp(f * m_GearNum, f * (m_GearNum + 1), Mathf.Abs(CurrentSpeed / MaxSpeed));
            m_GearFactor = Mathf.Lerp(m_GearFactor, targetGearFactor, Time.deltaTime * 5f);
        }

        private void CalculateRevs()
        {
            // calculate engine revs (for display / sound)
            // (this is done in retrospect - revs are not used in force/power calculations)
            CalculateGearFactor();
            var gearNumFactor = m_GearNum / (float)NoOfGears;
            var revsRangeMin = ULerp(0f, m_RevRangeBoundary, CurveFactor(gearNumFactor));
            var revsRangeMax = ULerp(m_RevRangeBoundary, 1f, gearNumFactor);
            Revs = ULerp(revsRangeMin, revsRangeMax, m_GearFactor);
        }

        private void CapSpeed()
        {
            var speed = _rigidbody.velocity.magnitude * 3.6f;

            if (speed > m_Topspeed) _rigidbody.velocity = (m_Topspeed / 3.6f) * _rigidbody.velocity.normalized;
        }

        private void ApplyDrive(float accel, float footbrake)
        {
            if (accel > 0 || footbrake > 0)
            {
                var thrustTorque = accel * m_CurrentTorque;
                var reverseTorque = -m_ReverseTorque * footbrake;

                Debug.Log("CurrentSpeed " + CurrentSpeed);

                for (int i = 0; i < _wheels.Length; i++)
                {
                    if (accel > 0f)
                    {
                        _wheels[i].SetBreakTorque(0f);
                        _wheels[i].SetMotorTorque(thrustTorque);
                    }
                    else if (CurrentSpeed > 0.05f && Vector3.Angle(_transform.forward, _rigidbody.velocity) < 90f)
                    {
                        _wheels[i].SetBreakTorque(m_MaxHandbrakeTorque * footbrake);
                    }
                    else if (footbrake > 0)
                    {
                        _wheels[i].SetBreakTorque(0f);
                        _wheels[i].SetMotorTorque(reverseTorque);
                    }
                }
            }
        }

        private void SteerHelper()
        {
            for (int i = 0; i < 4; i++)
            {
                _wheels[i].GetWheelGroundHit(out var wheelHit);

                if (wheelHit.normal == Vector3.zero)
                    return; // wheels arent on the ground so dont realign the rigidbody velocity
            }

            // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            if (Mathf.Abs(m_OldRotation - _transform.eulerAngles.y) < 10f)
            {
                var turnadjust = (_transform.eulerAngles.y - m_OldRotation) * m_SteerHelper;
                Quaternion velRotation = Quaternion.AngleAxis(turnadjust, Vector3.up);
                _rigidbody.velocity = velRotation * _rigidbody.velocity;
            }
            m_OldRotation = _transform.eulerAngles.y;
        }

        // this is used to add more grip in relation to speed
        private void AddDownForce() => _rigidbody.AddForce(-_transform.up * m_Downforce * _rigidbody.velocity.magnitude);

        // checks if the wheels are spinning and is so does three things
        // 1) emits particles
        // 2) plays tiure skidding sounds
        // 3) leaves skidmarks on the ground
        // these effects are controlled through the WheelEffects class
        private void CheckForWheelSpin()
        {
            // loop through all wheels
            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i].GetWheelGroundHit(out var wheelHit);

                // is the tire slipping above the given threshhold
                //if (Mathf.Abs(wheelHit.forwardSlip) >= m_SlipLimit || Mathf.Abs(wheelHit.sidewaysSlip) >= m_SlipLimit)
                //{
                //    m_WheelEffects[i].EmitTyreSmoke();

                //    // avoiding all four tires screeching at the same time
                //    // if they do it can lead to some strange audio artefacts
                //    if (!AnySkidSoundPlaying())
                //    {
                //        m_WheelEffects[i].PlayAudio();
                //    }
                //    continue;
                //}

                //// if it wasnt slipping stop all the audio
                //if (m_WheelEffects[i].PlayingAudio)
                //{
                //    m_WheelEffects[i].StopAudio();
                //}
                //// end the trail generation
                //m_WheelEffects[i].EndSkidTrail();
            }
        }

        // crude traction control that reduces the power to wheel if the car is wheel spinning too much
        private void TractionControl()
        {
            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i].GetWheelGroundHit(out var wheelHit);

                AdjustTorque(wheelHit.forwardSlip);
            }
        }

        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= m_SlipLimit && m_CurrentTorque >= 0)
            {
                m_CurrentTorque -= 10 * m_TractionControl;
            }
            else
            {
                m_CurrentTorque += 10 * m_TractionControl;

                if (m_CurrentTorque > m_FullTorqueOverAllWheels)
                {
                    m_CurrentTorque = m_FullTorqueOverAllWheels;
                }
            }
        }

        private bool AnySkidSoundPlaying()
        {
            //for (int i = 0; i < 4; i++)
            //{
            //    if (m_WheelEffects[i].PlayingAudio)
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
        #endregion
    }
}