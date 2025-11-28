using Items.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Items.Core
{
    public class ItemDropper : MonoBehaviour
    {
        private static ItemDropper instance;
        
        [SerializeField] private GameObject itemPickupPrefab;
        [SerializeField] private Transform dropLocation;
        [SerializeField] private float dropForce = 3f;
        
        public static bool IsActive => instance != null && instance.gameObject.activeSelf;

        private void Awake()
        {
            instance = this;
            Hide();
        }

        public static void Show()
        {
            if (instance != null)
                instance.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            if (instance != null)
                instance.gameObject.SetActive(false);
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            InvItemUI fromItemUI = eventData.pointerDrag?.GetComponent<InvItemUI>();
            if (fromItemUI == null) return;
            
            InvSlotUI slotUI = fromItemUI.SlotUI;
            if (slotUI == null) return;
            
            ItemAmount itemToDrop = new ItemAmount(fromItemUI.ItemAmount);
            
            Drop(fromItemUI.ItemAmount);
            
            ItemAmount empty = new ItemAmount();
            slotUI.InvView.InventorySystem.RemoveItem(ref itemToDrop);
            
            Hide();
        }

        public static void Drop(ItemAmount itemAmount)
        {
            if (instance == null || itemAmount.IsEmpty) return;
            
            Vector3 dropPos = instance.dropLocation != null 
                ? instance.dropLocation.position 
                : Vector3.zero;
            
            GameObject itemPickup = Instantiate(instance.itemPickupPrefab, dropPos, Quaternion.identity);
            ItemPickup pickup = itemPickup.GetComponent<ItemPickup>();
            
            if (pickup != null)
            {
                pickup.SetItemAmount(new ItemAmount(itemAmount));
            }
            
            Rigidbody2D rb = itemPickup.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * instance.dropForce, ForceMode2D.Impulse);
            }
        }
    }
}
