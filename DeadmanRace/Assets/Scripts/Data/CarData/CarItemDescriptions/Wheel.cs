using UnityEngine;
using DeadmanRace.Enums;
using DeadmanRace.Components;

namespace DeadmanRace.Items
{
    [CreateAssetMenu(fileName = "New Wheel", menuName = "Items/Car/Create wheel")]
    public class Wheel : CarItemDescription
    {
        [SerializeField]
        private bool _isDrivable = true;

        [SerializeField]
        private bool _isSteerActive = false;

        [SerializeField]
        private bool _isMirrorRotation = false;

        public bool IsDrivable { get => _isDrivable; }

        public bool IsSteerActive { get => _isSteerActive; }

        public bool IsMirrorRotation { get => _isMirrorRotation; }

        public override void InstantiateObject(Transform parent, Vector3 position)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.AddComponent<WheelCollider>();

            if (!_createEmpty) obj.AddComponent<CarWheel>().Initialize(this);
            else obj.AddComponent<CarWheel>().Initialize(ItemType);
        }

        protected override void OnEnable() => ItemType = ItemTypes.Wheel;
    }
}