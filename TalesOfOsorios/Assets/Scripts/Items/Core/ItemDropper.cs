using Items.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using Managers;

namespace Items.Core
{
    public class ItemDropper : MonoBehaviour, IDropHandler
    {
        private static ItemDropper _instance;
        
        public static bool IsActive => _instance != null && _instance.gameObject.activeSelf;

        private void Awake()
        {
            _instance = this;
            Hide();
        }

        public static void Show() => _instance.gameObject.SetActive(true);
        public static void Hide() => _instance.gameObject.SetActive(false);
        
        public void OnDrop(PointerEventData eventData)
        {
            InvItemUI fromItemUI = eventData.pointerDrag?.GetComponent<InvItemUI>();
            if (fromItemUI == null) return;
            
            InvSlotUI slotUI = fromItemUI.SlotUI;
            if (slotUI == null) return;
            
            ItemAmount itemToDrop = new ItemAmount(fromItemUI.ItemAmount);
            
            Drop(fromItemUI.ItemAmount);
            
            slotUI.InvView.InventorySystem.RemoveItem(ref itemToDrop);
            
            Hide();
        }

        public static void Drop(ItemAmount itemAmount)
        {
            if (_instance == null || itemAmount.IsEmpty) return;
            
            Vector3 dropPosition = GameManager.Player.transform.position + GameManager.Player.transform.forward * 2f;
            GameObject droppedItem = Instantiate(PrefabsManager.ItemPrefabPickup, dropPosition, Quaternion.identity);
            ItemPickup pickup = droppedItem.GetComponent<ItemPickup>();
        }
    }
}
