using Interfaces;
using UnityEngine;
using Managers;

namespace Items.Core
{
    public class ItemPickup : MonoBehaviour, IInteractable
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

        public void Interact()
        {
            InvSystem baseInventory = GameManager.Canvas.InvManager.BaseInventory;

            TryPickup(baseInventory);
        }
        
        public bool CanInteract()
        {
            return !itemAmount.IsEmpty;
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
