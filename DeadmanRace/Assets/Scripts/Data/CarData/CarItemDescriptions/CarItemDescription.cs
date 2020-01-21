using UnityEngine;
using DeadmanRace.Enums;
using DeadmanRace.Interfaces;


namespace DeadmanRace.Items
{
    public abstract class CarItemDescription : ScriptableObject, IItemDescription
    {
        #region Fields

        [SerializeField] private int _iD;
        [SerializeField] private Sprite _icon;
        [SerializeField] private float _weight;
        [SerializeField] private Sprite _gameSprite;
        [SerializeField] protected bool _createEmpty = false;

        #endregion


        #region Properties

        public int ID { get => _iD; }

        public ItemTypes ItemType { get; protected set;}

        public Sprite Icon { get => _icon; }

        public float Weight { get => _weight; }

        public Sprite GameSprite { get => _gameSprite; }

        #endregion


        #region UnityMethods

        protected abstract void OnEnable();

        #endregion


        #region Methods

        public abstract void InstantiateObject(Transform parent, Vector3 position);

        #endregion
    }
}