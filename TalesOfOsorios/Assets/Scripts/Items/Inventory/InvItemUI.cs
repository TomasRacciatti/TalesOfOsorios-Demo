using Items.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items.Inventory
{
    public class InvItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, 
        IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;
        
        private ItemAmount _itemAmount;
        private InvSlotUI _slotUI;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        
        public ItemAmount ItemAmount => _itemAmount;
        public InvSlotUI SlotUI => _slotUI;
        
        private void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _canvas = GetComponent<Canvas>();
            if (_canvas == null)
            {
                _canvas = gameObject.AddComponent<Canvas>();
                _canvas.overrideSorting = true;
            }
            _canvas.sortingOrder = 10;
            
            _rectTransform = GetComponent<RectTransform>();
            
            if (amountText != null)
                amountText.raycastTarget = false;
        }

        public void SetSlotUI(InvSlotUI invSlotUI)
        {
            _slotUI = invSlotUI;
        }
        
        public void ShowItem(ItemAmount newItemAmount)
        {
            _itemAmount = newItemAmount;
            
            if (icon != null)
            {
                icon.enabled = !_itemAmount.IsEmpty;
                icon.sprite = _itemAmount.IsEmpty ? null : _itemAmount.SoItem.Icon;
            }
            
            RefreshAmount();
        }
        
        private void RefreshAmount()
        {
            if (amountText == null) return;
            
            amountText.SetText(_itemAmount.Amount.ToString());
            amountText.gameObject.SetActive(_itemAmount.Amount > 1);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_itemAmount.IsEmpty) return;
            
            icon.raycastTarget = false;
            _canvas.sortingOrder = 15;
            
            ItemDropper.Show();
            
            ItemsTooltip.Hide();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_itemAmount.IsEmpty) return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                transform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera, 
                out Vector2 localPoint);
            transform.localPosition = localPoint;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            icon.raycastTarget = true;
            _canvas.sortingOrder = 10;
            transform.localPosition = Vector3.zero;
            
            ItemDropper.Hide();
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_itemAmount.IsEmpty)
                ItemsTooltip.Show(_itemAmount);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemsTooltip.Hide();
        }
    }
}
