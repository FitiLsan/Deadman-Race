using UnityEngine;
using UnityEngine.EventSystems;

namespace DeadmanRace.Interfaces
{
    public interface IDragAndDropComponent
    {
        void EndDragging();
        void SetDraggingPosition(PointerEventData data);
        void StartDragging(Sprite draggingIcon, Vector2 size);
    }
}