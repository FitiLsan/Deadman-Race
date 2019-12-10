using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DeadmanRace.Interfaces;
using DeadmanRace;

namespace DeadmanRace.UI
{
    public class DragAndDropComponent : MonoBehaviour, IDragAndDropHelper
    {
        private Canvas _canvas;
        private GameObject _draggingObj;
        private Image _draggingIcon;
        private RectTransform _draggingObjRectTransform;
        private RectTransform _draggingPlane;

        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _draggingObj = new GameObject("DraggingIcon");
            _draggingObj.transform.SetParent(_canvas.transform, false);
            _draggingObj.transform.SetAsLastSibling();

            _draggingIcon = _draggingObj.AddComponent<Image>();
            _draggingIcon.raycastTarget = false;

            _draggingObj.SetActive(false);

            _draggingPlane = _canvas.transform as RectTransform;
        }

        public void StartDragging(Sprite draggingIcon, Vector2 size)
        {
            _draggingIcon.sprite = draggingIcon;
            _draggingObj.SetActive(true);
            _draggingObjRectTransform = _draggingObj.GetComponent<RectTransform>();
            _draggingObjRectTransform.sizeDelta = size;
        }

        public void SetDraggingPosition(PointerEventData data)
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_draggingPlane, data.position, data.pressEventCamera, out var globalMousePos))
            {
                _draggingObjRectTransform.position = globalMousePos;
                _draggingObjRectTransform.rotation = _draggingPlane.rotation;
            }
        }

        public void EndDragging()
        {
            _draggingObj.SetActive(false);
            _draggingIcon.sprite = null;
        }
    }
}
