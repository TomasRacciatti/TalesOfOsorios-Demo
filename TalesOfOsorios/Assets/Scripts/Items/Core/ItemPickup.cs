using UnityEngine;

namespace Items.Core
{
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private ItemAmount itemAmount;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public ItemAmount ItemAmount => itemAmount;

        private void Start()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (spriteRenderer != null && itemAmount != null && !itemAmount.IsEmpty)
            {
                spriteRenderer.sprite = itemAmount.SoItem.Icon;
            }
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
