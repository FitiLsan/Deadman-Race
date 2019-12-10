using DeadmanRace.Enums;

namespace DeadmanRace.Interfaces
{
    public interface IEquipableComponent
    {
        ItemTypes _componentType { get; }

        bool AttachSlot(IEquipmentSlot slot);

        void UnattachSlot();
    }
}