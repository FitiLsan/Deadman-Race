using UnityEngine;
using DeadmanRace.Enums;

namespace DeadmanRace.Items
{
    public class Generator : CarItemDescription
    {
        #region Fields

        [SerializeField]
        [Range(0f, 1f)]
        private float _reducePowerByGenerator;

        [SerializeField]
        [Range(0f, 5f)]
        private float _electricSlotsAmount;

        #endregion


        #region Properties

        public float ReducePowerByGenerator { get => 1f - _reducePowerByGenerator; }

        public float ElectricSlotsAmount { get => _electricSlotsAmount; }

        #endregion


        #region UnityMethods

        protected override void OnEnable() => ItemType = ItemTypes.Generator;

        #endregion


        #region Methods

        public override void InstantiateObject(Transform parent, Vector3 position)
        {
            // here will be some logic of creation generator 
        }

        #endregion
    }
}