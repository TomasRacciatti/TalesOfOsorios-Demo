using UnityEngine;
using UnityEngine.EventSystems;
using Items.Core;
using Items.Inventory;

public class InvSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject itemUIPrefab;
        
    private InvView invView;
    private int invSlot;
        
    public InvView InvView => invView;
    public int InvSlot => invSlot;
        
    public void Initialize(InvView inventoryView, int inventorySlot)
    {
        invView = inventoryView;
        invSlot = inventorySlot;
    }
        
    public void SetItem(ItemAmount itemAmount)
    {
        InvItemUI itemUI = GetComponentInChildren<InvItemUI>();
            
        if (itemAmount.IsEmpty)
        {
            if (itemUI != null)
            {
                Destroy(itemUI.gameObject);
            }
            return;
        }

        if (itemUI == null)
        {
            // Changed: Use serialized prefab reference instead of manager
            GameObject newItem = Instantiate(itemUIPrefab, transform);
            itemUI = newItem.GetComponent<InvItemUI>();
            itemUI.SetSlotUI(this);
                
            RectTransform rect = newItem.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.localScale = Vector3.one;
        }

        itemUI.ShowItem(itemAmount);
    }
        
    public void OnDrop(PointerEventData eventData)
    {
        InvItemUI fromItemUI = eventData.pointerDrag?.GetComponent<InvItemUI>();
        if (fromItemUI == null) return;
            
        InvSlotUI fromSlotUI = fromItemUI.SlotUI;
        if (fromSlotUI == null) return;

        ItemsTooltip.Hide();

        // Changed: Use the new transfer method
        fromSlotUI.InvView.InventorySystem.TransferIndexToIndex(
            InvView.InventorySystem, 
            fromSlotUI.InvSlot, 
            InvSlot
        );
    }
}
