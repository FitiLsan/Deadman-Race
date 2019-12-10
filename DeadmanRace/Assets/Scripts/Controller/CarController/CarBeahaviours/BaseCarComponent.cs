using UnityEngine;
using DeadmanRace.Interfaces;
using DeadmanRace.Enums;

namespace DeadmanRace.Components
{
    public abstract class BaseCarComponent<T> : MonoBehaviour, IEquipableComponent, IWeightComponent where T : class, IItemDescription
    {
        protected T _description;

        protected IEquipmentSlot _subscribedSlot;

        protected bool _descriptionIsNull = true;

        protected bool _subscribedSlotIsNull = true;

        protected bool _isSunscribed = false;


        public ItemTypes _componentType { get; private set; }


        protected virtual void SetItem(IItemDescription description)
        {
            _description = description as T;
            if (_description != null) _descriptionIsNull = false;
        }

        protected virtual void ClearItem()
        {
            _description = null;
            _descriptionIsNull = true;
        }

        public virtual bool AttachSlot(IEquipmentSlot slot)
        {
            if (_isSunscribed) return false;

            _subscribedSlot = slot;
            _subscribedSlotIsNull = false;

            _subscribedSlot.Equip(_description);

            _subscribedSlot.OnChange += UpdateComponent;

            _subscribedSlot.IsActive = true;

            _isSunscribed = true;

            return true;
        }

        public virtual void UnattachSlot()
        {
            _subscribedSlot.IsActive = false;

            _subscribedSlot.OnChange -= UpdateComponent;

            _subscribedSlot.Unequip();

            _subscribedSlot = null;
            _subscribedSlotIsNull = true;
            _isSunscribed = false;
        }

        protected virtual void UpdateComponent(IItemDescription data, EquipmentEventTypes evenType)
        {
            switch (evenType)
            {
                case EquipmentEventTypes.Equip:
                    SetItem(data);
                    break;

                case EquipmentEventTypes.Unequip:
                    ClearItem();
                    break;

                default:

                    break;
            }
        }

        public virtual void Initialize(ItemTypes type) => _componentType = type;

        public virtual void Initialize(T description)
        {
            _componentType = description.ItemType;

            SetItem(description);
        }
        
        public virtual float GetWeight() => _descriptionIsNull ? 0f : _description.Weight;
    }
}