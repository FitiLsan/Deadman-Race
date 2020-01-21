using DeadmanRace.Enums;
using UnityEngine;

namespace DeadmanRace.Interfaces
{
    public interface IItemDescription
    {
        int ID { get; }
        Sprite Icon { get; }
        ItemTypes ItemType { get; }
        float Weight { get; }
    }
}