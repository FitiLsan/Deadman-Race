using DeadmanRace.Interfaces;
using DeadmanRace.Enums;
using System;

namespace DeadmanRace.Objects
{
    public class EquipmentSlot : IEquipmentSlot
    {
        public event Action<IItemDescription, EquipmentEventTypes> OnChange;

        #region Fields

        private const int EMPTY = -1;

        private bool _isActive;

        #endregion


        #region Properties

        public IItemDescription Item { get; private set; }
        public ItemTypes Type { get; private set; }
        public bool IsEmpty { get; private set; } = true;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;

                if (_isActive) OnChange?.Invoke(Item, EquipmentEventTypes.SlotEnabled);
                else OnChange?.Invoke(Item, EquipmentEventTypes.SlotDisabled);
            }
        }

        #endregion


        #region ClassLifeCycles

        public EquipmentSlot(ItemTypes slotType)
        {
            Type = slotType;
        }

        #endregion


        #region IEquipmentSlot

        public bool Equip(IItemDescription item)
        {
            if (item == null) return false;

            if (item.ItemType != Type) return false;

            Item = item;
            IsEmpty = false;

            OnChange?.Invoke(Item, EquipmentEventTypes.Equip);

            return true;
        }

        public void Unequip()
        {
            Item = null;
            IsEmpty = true;

            OnChange?.Invoke(Item, EquipmentEventTypes.Unequip);
        }

        #endregion
    }
}
