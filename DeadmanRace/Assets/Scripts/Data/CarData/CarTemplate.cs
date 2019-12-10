using System.Collections.Generic;
using UnityEngine;
using DeadmanRace.Enums;
using DeadmanRace.Components;

namespace DeadmanRace.Items
{
    [CreateAssetMenu(fileName = "New car template", menuName = "Data/Car/Create template")]
    public class CarTemplate : ScriptableObject
    {
        #region SerializeFields

        [SerializeField]
        private string _name = "Car";

        [Space(10)]
        [SerializeField]
        private Vector3 _carRotation = Vector3.zero;

        [Space(10)]
        [SerializeField]
        private Vector3 _carcasePosition = Vector3.zero;

        [SerializeField]
        private Carcase _carcaseDescription;

        [Space(10)]
        [SerializeField]
        private Vector3 _enginePosition = Vector3.zero;

        [SerializeField]
        private Engine _engineDescription;

        [Space(10)]
        [SerializeField]
        private Vector3 _fuelTankPosition = Vector3.zero;

        [SerializeField]
        private FuelTank _fuelTankDescription;

        [Space(10)]
        [SerializeField]
        private Vector3[] _wheelsPositions;

        [SerializeField]
        private Wheel[] _wheelDescriptions;

        [Space(10)]
        [SerializeField]
        private Generator _generatorDescriptions;

        [Space(10)]
        [SerializeField]
        private CarItemDescription[] _electronics = new CarItemDescription[5];

        [Space(10)]
        [SerializeField]
        private CarItemDescription[] _weapons = new CarItemDescription[6];
        #endregion

        #region SerializeFieldsFromStandartAssetPack
        [Space(20)]
        [SerializeField]
        [Range(0, 1)]
        private float m_SteerHelper;
        public float GetSteerHelper { get => m_SteerHelper; }

        [SerializeField]
        private float m_MaximumSteerAngle;
        public float GetMaximumSteerAngle { get => m_MaximumSteerAngle; }

        [SerializeField]
        [Range(0, 1)]
        private float m_TractionControl;
        public float GetTractionControl { get => m_TractionControl; }

        [SerializeField]
        private Vector3 m_CentreOfMassOffset;
        public Vector3 GetCentreOfMassOffset { get => m_CentreOfMassOffset; }

        [SerializeField]
        private float m_ReverseTorque;
        public float GetReverseTorque { get => m_ReverseTorque; }

        [SerializeField]
        private float m_Downforce = 100f;
        public float GetDownforce { get => m_Downforce; }

        [SerializeField]
        private float m_Topspeed = 200;
        public float GetTopspeed { get => m_Topspeed; }

        [SerializeField]
        private int m_NoOfGears = 5;
        public int GetNoOfGears { get => m_NoOfGears; }

        [SerializeField]
        private float m_RevRangeBoundary = 1f;
        public float GetRevRangeBoundary { get => m_RevRangeBoundary; }

        [SerializeField]
        private float m_SlipLimit;
        public float GetSlipLimit { get => m_SlipLimit; }

        [SerializeField]
        private float m_BrakeTorque;
        public float GetBrakeTorque { get => m_BrakeTorque; }

        #endregion

        private float _mass;

        public Transform InstantiateCar()
        {
            var carTransform = new GameObject(_name).transform;

            _mass = 0f;
            
            _mass += InstantiateComponent(_carcaseDescription, carTransform, _carcasePosition);
            _mass += InstantiateComponent(_engineDescription, carTransform, _enginePosition);
            _mass += InstantiateComponent(_fuelTankDescription, carTransform, _fuelTankPosition);
            _mass += InstantiateComponent(_generatorDescriptions, carTransform, Vector3.zero);
            
            for (var i = 0; i < _wheelsPositions.Length; i++)
                _mass += InstantiateComponent(_wheelDescriptions[i], carTransform, _wheelsPositions[i]);
            
            for (var i = 0; i < _electronics.Length; i++)
                _mass += InstantiateComponent(_electronics[i], carTransform, Vector3.zero);
            
            for (var i = 0; i < _weapons.Length; i++)
                _mass += InstantiateComponent(_weapons[i], carTransform, Vector3.zero);
            
            var rigidbody = carTransform.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.mass = _mass;

            carTransform.localEulerAngles = _carRotation;

            return carTransform;
        }

        private float InstantiateComponent(CarItemDescription component, Transform parent, Vector3 position)
        {
            if (component == null) return 0f;

            component.InstantiateObject(parent, position);

            return component.Weight;
        }
    }
}