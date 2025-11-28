using UnityEngine;

namespace Items.Core
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private ItemAmount itemAmount;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float pickupRadius = 1.5f;
        
        public ItemAmount ItemAmount => itemAmount;

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetItemAmount(ItemAmount newItemAmount)
        {
            itemAmount = new ItemAmount(newItemAmount);
            
            if (spriteRenderer != null && !itemAmount.IsEmpty)
            {
                spriteRenderer.sprite = itemAmount.SoItem.Icon;
            }
        }
        
        public void TryPickup(InvSystem playerInventory)
        {
            if (itemAmount.IsEmpty || playerInventory == null) return;
            
            ItemAmount tempItem = new ItemAmount(itemAmount);
            bool success = playerInventory.AddItem(ref tempItem);
            
            if (tempItem.IsEmpty)
            {
                Destroy(gameObject);
            }
            else
            {
                itemAmount = tempItem;
            }
        }
    }
}
