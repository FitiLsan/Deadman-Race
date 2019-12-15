using UnityEngine;
using DeadmanRace.Enums;
using DeadmanRace.Components;


namespace DeadmanRace.Items
{
    [CreateAssetMenu(fileName = "New Carcase", menuName = "Data/Car/Components/Create carcase")]
    public class Carcase : CarItemDescription
    {
        #region Fields

        [SerializeField] private Vector3 _hitboxSize;
        [SerializeField] private float _maxHealth;
        [SerializeField] private Vector3 _spriteScale = Vector3.one;

        private Vector3 _spriteRotation = new Vector3(90f, 0f, 0f);

        #endregion


        #region Properties

        public float MaxHealth { get => _maxHealth; }

        #endregion


        #region UnityMethods

        protected override void OnEnable() => ItemType = ItemTypes.Carcase;

        #endregion


        #region Methods

        public override void InstantiateObject(Transform parent, Vector3 position)
        {
            var carObject = new GameObject(name);
            carObject.transform.SetParent(parent);

            var collider = carObject.AddComponent<BoxCollider>(); 
            collider.size = _hitboxSize;
            collider.center = position;

            var spriteObj = new GameObject();
            spriteObj.transform.SetParent(carObject.transform);
            spriteObj.transform.localEulerAngles = _spriteRotation;
            spriteObj.transform.localScale = _spriteScale;
            spriteObj.AddComponent<SpriteRenderer>().sprite = GameSprite;

            if (!_createEmpty) carObject.AddComponent<CarCarcase>().Initialize(this);
            else carObject.AddComponent<CarCarcase>().Initialize(ItemType);
        }

        #endregion
    }
}