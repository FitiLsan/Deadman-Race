using UnityEngine;
using DeadmanRace.Enums;

namespace DeadmanRace.Items
{
    public class Generator : CarItemDescription
    {
        [Range(0f, 1f)]
        private float _reducePowerByGenerator;

        public float ReducePowerByGenerator { get => 1f - _reducePowerByGenerator; }

        [Range(0f, 5f)]
        private float _electricSlotsAmount;

        public float ElectricSlotsAmount { get => _electricSlotsAmount; }

        protected override void OnEnable()
        {
            ItemType = ItemTypes.Generator;
        }

        public override void InstantiateObject(Transform parent, Vector3 position)
        {
            throw new System.NotImplementedException();
        }
    }
}