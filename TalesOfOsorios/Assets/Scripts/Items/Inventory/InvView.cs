using Items.Core;
using UnityEngine;

namespace Items.Inventory
{
    public class InvView : MonoBehaviour
    {
        [SerializeField] private InvSlotUI[] slots;
        [SerializeField] private InvSystem inventorySystem;
        
        public InvSystem InventorySystem => inventorySystem;

        private void Awake()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Initialize(this, i);
            }
        }

        private void Start()
        {
            if (inventorySystem != null)
                SetInventory(inventorySystem);
        }
        
        public void SetInventory(InvSystem newInventory)
        {
            if (inventorySystem != null)
                inventorySystem.OnItemChanged -= OnItemChanged;

            inventorySystem = newInventory;

            if (inventorySystem == null) return;

            inventorySystem.OnItemChanged += OnItemChanged;
            UpdateAllSlots();
        }

        private void OnItemChanged(int index, ItemAmount item)
        {
            if (index < 0 || index >= slots.Length) return;
            slots[index].SetItem(item);
        }

        private void UpdateAllSlots()
        {
            var items = inventorySystem.Items;

            for (int i = 0; i < slots.Length; i++)
            {
                if (i < items.Count)
                    slots[i].SetItem(items[i]);
            }
        }
    }
}
