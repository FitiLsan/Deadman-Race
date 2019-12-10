using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DeadmanRace.Interfaces;
using DeadmanRace.Enums;

namespace DeadmanRace.UI
{
    public class SlotUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private const float ALPHA_TRANSPARENT = 0.5f;
        private const float ALPHA_OPAQUE = 1f;

        [SerializeField]
        private ItemTypes _slotType;
        
        private Image _slotIcon;

        private IconUI _icon;
        private bool _iconIsNull = true;

        private IOutlineHelper _outline;
        private bool _outlineIsNull = true;

        private IDragAndDropHelper _dragAndDropHelper;
        private bool _dragAndDropHelperIsNull = true;

        private bool _slotIsActive = false;

        public ItemTypes GetSlotType { get => _slotType; }

        public IEquipmentSlot Slot { get; private set; }

        private void Awake()
        {
            _slotIcon = GetComponent<Image>();
            if (_slotIcon == null) throw new System.NullReferenceException("[SlotUI] Image component not found");

            _dragAndDropHelper = GetComponentInParent<IDragAndDropHelper>();
            if (_dragAndDropHelper != null) _dragAndDropHelperIsNull = false;

            _outline = GetComponentInChildren<IOutlineHelper>();
            if (_outline != null) _outlineIsNull = false;

            _icon = GetComponentInChildren<IconUI>();
            if (_icon != null) _iconIsNull = false;

            SetActive(false);
        }

        public void AttachEquipmentSlot(IEquipmentSlot slot) => Slot = slot;
        
        public void UpdateSlot(IItemDescription item, EquipmentEventTypes eventType)
        {
            switch (eventType)
            {
                case EquipmentEventTypes.Equip:
                    if (_iconIsNull) return;
                    _icon.Set(item.Icon);
                    break;

                case EquipmentEventTypes.Unequip:
                    if (_iconIsNull) return;
                    _icon.Clear();
                    break;

                case EquipmentEventTypes.SlotEnabled:
                    SetActive(true);
                    break;

                case EquipmentEventTypes.SlotDisabled:
                    SetActive(false);
                    break;

                default:
                    break;
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_outlineIsNull || !_slotIsActive) return;

            _outline.Enable();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_outlineIsNull || !_slotIsActive) return;

            _outline.Disable();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_dragAndDropHelperIsNull) return;

            _icon.Enabled = false;

            var size = (_icon.transform as RectTransform).sizeDelta;
            
            _dragAndDropHelper.StartDragging(_icon.GetIcon, size);

            _dragAndDropHelper.SetDraggingPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_dragAndDropHelperIsNull) return;

            _dragAndDropHelper.SetDraggingPosition(eventData);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_dragAndDropHelperIsNull) return;

            _dragAndDropHelper.EndDragging();

            var pointerObj = eventData.pointerCurrentRaycast.gameObject;

            if (pointerObj == null)
            {
                _icon.Enabled = true;
                return;
            }

            if (pointerObj.TryGetComponent<SlotUI>(out var uiSlot))
            {
                if (uiSlot.Slot.IsEmpty)
                {
                    if (uiSlot.Slot.Equip(Slot.Item)) Slot.Unequip();
                    else _icon.Enabled = true;
                }
                else
                {
                    var temp = uiSlot.Slot.Item;
                    if (uiSlot.Slot.Equip(Slot.Item)) Slot.Equip(temp);
                    else _icon.Enabled = true;
                }
            }
        }

        
        private void SetActive(bool isActive)
        {
            _slotIsActive = isActive;
            var curentColor = _slotIcon.color;

            if (isActive)
            {
                curentColor.a = ALPHA_OPAQUE;
                _slotIcon.color = curentColor;
            }
            else
            {
                curentColor.a = ALPHA_TRANSPARENT;
                _slotIcon.color = curentColor;
            }
        }
    }
}
