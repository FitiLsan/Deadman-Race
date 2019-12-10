using UnityEngine;
using UnityEngine.EventSystems;

namespace DeadmanRace.Interfaces
{
    public interface IDragAndDropHelper
    {
        void EndDragging();
        void SetDraggingPosition(PointerEventData data);
        void StartDragging(Sprite draggingIcon, Vector2 size);
    }
}