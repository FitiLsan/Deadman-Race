using System;
using DeadmanRace.Enums;

namespace DeadmanRace.Interfaces
{
    public interface IEquipmentSlot
    {
        event Action<IItemDescription, EquipmentEventTypes> OnChange;

        IItemDescription Item { get; }
        ItemTypes Type { get; }
        bool IsActive { get; set; }
        bool IsEmpty { get; }
        
        bool Equip (IItemDescription item);
        void Unequip ();
    }
}
