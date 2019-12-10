using UnityEngine;
using DeadmanRace.Enums;
using DeadmanRace.Components;

namespace DeadmanRace.Items
{
    [CreateAssetMenu(fileName = "New fuel tank", menuName = "Items/Car/Create fuel tank")]
    public class FuelTank : CarItemDescription
    {
        [SerializeField]
        private Vector3 _hitboxSize;

        [SerializeField]
        private float _capacity;

        [SerializeField]
        private float _health;

        public float Capacity { get => _capacity; }

        public float Health { get => _health; }

        public override void InstantiateObject(Transform parent, Vector3 position)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);

            var collider = obj.AddComponent<BoxCollider>();
            collider.size = _hitboxSize;
            collider.center = position;
            collider.isTrigger = true;

            if (!_createEmpty) obj.AddComponent<CarFuelTank>().Initialize(this);
            else obj.AddComponent<CarFuelTank>().Initialize(ItemType);
        }

        protected override void OnEnable() => ItemType = ItemTypes.FuelTank;
    }
}