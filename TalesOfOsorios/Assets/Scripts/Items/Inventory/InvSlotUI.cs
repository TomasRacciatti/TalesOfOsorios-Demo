using UnityEngine;
using UnityEngine.EventSystems;
using Items.Core;
using Managers;

namespace Items.Inventory
{
    public class InvSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private EquipSlotId requiredSlotId = EquipSlotId.None;
        
        private InvView invView;
        private int invSlot;

        public InvView InvView => invView;
        public int InvSlot => invSlot;
        public EquipSlotId RequiredSlotId => requiredSlotId;

        public void Initialize(InvView inventoryView, int inventorySlot)
        {
            invView = inventoryView;
            invSlot = inventorySlot;
        }

        public void SetItem(ItemAmount itemAmount)
        {
            InvItemUI itemUI = GetComponentInChildren<InvItemUI>();

            if (itemAmount == null || itemAmount.IsEmpty)
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

            if (requiredSlotId != EquipSlotId.None)
            {
                ItemAmount fromItem = fromItemUI.ItemAmount;
                if (fromItem == null || fromItem.IsEmpty) return;
                
                if (fromItem.SoItem.EquipSlotId != requiredSlotId)
                {
                    Debug.Log($"Cannot equip {fromItem.SoItem.ItemName} in {requiredSlotId} slot");
                    ItemDropper.Hide();
                    return;
                }
            }

            ItemsTooltip.Hide();
            ItemDropper.Hide();

            fromSlotUI.InvView.InventorySystem.TransferIndexToIndex(
                InvView.InventorySystem, fromSlotUI.InvSlot, InvSlot);
        }
    }
    
    public enum EquipSlotId
    {
        None,
        Potion,
        Chest,
        Waist,
        Arms,
        Hands,
        Shoulder,
        Legs
    }
}