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
        
        private ItemAmount itemAmount;
        private InvSlotUI slotUI;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        
        public ItemAmount ItemAmount => itemAmount;
        public InvSlotUI SlotUI => slotUI;
        
        private void Awake()
        {
            canvas = GetComponentInParent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                
            rectTransform = GetComponent<RectTransform>();
            
            if (amountText != null)
                amountText.raycastTarget = false;
        }

        public void SetSlotUI(InvSlotUI invSlotUI)
        {
            slotUI = invSlotUI;
        }
        
        public void ShowItem(ItemAmount newItemAmount)
        {
            itemAmount = newItemAmount;
            
            if (icon != null)
            {
                icon.enabled = !itemAmount.IsEmpty;
                icon.sprite = itemAmount.IsEmpty ? null : itemAmount.SoItem.Icon;
            }
            
            RefreshAmount();
        }
        
        private void RefreshAmount()
        {
            if (amountText == null) return;
            
            amountText.SetText(itemAmount.Amount.ToString());
            amountText.gameObject.SetActive(itemAmount.Amount > 1);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount.IsEmpty) return;
            
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            
            ItemsTooltip.Hide();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (itemAmount.IsEmpty) return;
            
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            rectTransform.anchoredPosition = Vector2.zero;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!itemAmount.IsEmpty)
                ItemsTooltip.Show(itemAmount);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemsTooltip.Hide();
        }
    }
}
