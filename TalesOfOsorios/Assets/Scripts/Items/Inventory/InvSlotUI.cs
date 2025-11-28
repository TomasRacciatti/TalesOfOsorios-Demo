using UnityEngine;
using UnityEngine.EventSystems;
using Items.Core;
using Items.Inventory;
using Managers;

public class InvSlotUI : MonoBehaviour, IDropHandler
{
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
            GameObject newItem = Instantiate(PrefabsManager.ItemPrefabUI, Vector3.zero, Quaternion.identity);
            itemUI = newItem.GetComponent<InvItemUI>();
            itemUI.SetSlotUI(this);
                
            newItem.transform.SetParent(transform, false);
            newItem.transform.localPosition = Vector3.zero;
            newItem.transform.localRotation = Quaternion.identity;
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

            fromSlotUI.InvView.InventorySystem.TransferIndexToIndex(
                InvView.InventorySystem, fromSlotUI.InvSlot, InvSlot);
    }
}
