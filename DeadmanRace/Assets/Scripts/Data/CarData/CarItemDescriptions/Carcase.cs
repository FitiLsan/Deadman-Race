using UnityEngine;
using DeadmanRace.Enums;
using DeadmanRace.Components;

namespace DeadmanRace.Items
{
    [CreateAssetMenu(fileName = "New Carcase", menuName = "Items/Car/Create carcase")]
    public class Carcase : CarItemDescription
    {
        [SerializeField]
        private Vector3 _hitboxSize;

        [SerializeField]
        private float _maxHealth;

        [SerializeField]
        private Vector3 _spriteScale = Vector3.one;

        private Vector3 _spriteRotation = new Vector3(90f, 0f, 0f);

        public float MaxHealth { get => _maxHealth; }

        public override void InstantiateObject(Transform parent, Vector3 position)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);

            var collider = obj.AddComponent<BoxCollider>(); 
            collider.size = _hitboxSize;
            collider.center = position;

            var spriteObj = new GameObject("Sprite");
            spriteObj.transform.SetParent(obj.transform);
            spriteObj.transform.localEulerAngles = _spriteRotation;
            spriteObj.transform.localScale = _spriteScale;
            spriteObj.AddComponent<SpriteRenderer>().sprite = GameSprite;

            if (!_createEmpty) obj.AddComponent<CarCarcase>().Initialize(this);
            else obj.AddComponent<CarCarcase>().Initialize(ItemType);
        }

        protected override void OnEnable() => ItemType = ItemTypes.Carcase;
    }
}