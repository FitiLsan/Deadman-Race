using System.Collections.Generic;
using UnityEngine;
using DeadmanRace.Interfaces;
using DeadmanRace.Enums;
using DeadmanRace.Items;
using DeadmanRace.Components;

namespace DeadmanRace.Models
{
    public sealed class CarModel : IDrivable
    {
        #region Fields

        public readonly ObjectType GameObjectType = ObjectType.Vehicle;
        
        private Rigidbody _rigidbody;

        private IWeightComponent[] _weightComponents;

        private MotionModel _motionModel;

        #endregion


        #region Properties

        public Transform CarTransform { get; private set; }

        #endregion


        #region ClassLifeCycles

        public CarModel(CarTemplate template)
        {
            CarTransform = template.InstantiateCar();

            _rigidbody = CarTransform.GetComponentInChildren<Rigidbody>();

            _weightComponents = CarTransform.GetComponentsInChildren<IWeightComponent>();

            _motionModel = new MotionModel(CarTransform, template);
        }

        #endregion


        #region Methods

        private void UpdateWeight()
        {
            var totalWeight = 0f;

            foreach (var weightComponent in _weightComponents)
            {
                totalWeight += weightComponent.GetWeight();
            }

            _rigidbody.mass = totalWeight;
        }

        #endregion


        #region IDrivable

        public void Drive(float steering, float accel, float footbrake, float handbrake)
        {
            UpdateWeight();
            _motionModel.Move(steering, accel, footbrake, handbrake);
        }
        
        #endregion
    }
}