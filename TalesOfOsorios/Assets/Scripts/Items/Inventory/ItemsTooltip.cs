using Items.Core;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Items.Inventory
{
    public class ItemsTooltip : MonoBehaviour
    {
        private static ItemsTooltip instance;
        
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text itemDescriptionText;
        [SerializeField] private Image itemIcon;
        
        private RectTransform rectTransform;
        private Canvas canvas;
        
        private void Awake()
        {
            instance = this;
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            Hide();
        }

        public static void Show(ItemAmount itemAmount)
        {
            if (instance == null || itemAmount.IsEmpty) return;
            
            instance.itemNameText.text = itemAmount.SoItem.ItemName;
            instance.itemDescriptionText.text = itemAmount.SoItem.Description;
            instance.itemIcon.sprite = itemAmount.SoItem.Icon;
            instance.gameObject.SetActive(true);
            instance.UpdatePosition();
        }

        public static void Hide()
        {
            if (instance != null)
                instance.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (gameObject.activeSelf)
                UpdatePosition();
        }
        
        private void UpdatePosition()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            
            Vector2 pivot = new Vector2(
                mousePos.x / Screen.width < 0.5f ? 0f : 1f,
                mousePos.y / Screen.height < 0.5f ? 0f : 1f
            );
            rectTransform.pivot = pivot;

            RectTransform parentRect = rectTransform.parent as RectTransform;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                mousePos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out var localPos
            );
            
            rectTransform.anchoredPosition = localPos;
        }
    }
}
