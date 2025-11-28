using UnityEngine;
using System;

namespace Items.Core
{
    [Serializable]
    public class ItemAmount
    {
        [SerializeField] private SoItem soItem;
        [SerializeField] private int amount;
        
        public SoItem SoItem => soItem;
        public int Amount => amount;
        public int Stack => soItem != null ? soItem.Stack : 0;
        public bool ValidSoItem => soItem != null;
        public bool IsEmpty => soItem == null || amount <= 0;
        public bool IsFull => soItem != null && amount >= Stack;
        

        public ItemAmount(ItemAmount newItemAmount)
        {
            soItem = newItemAmount.SoItem;
            amount = newItemAmount.Amount;
        }

        public ItemAmount(SoItem newSoItem = null, int newAmount = 0)
        {
            soItem = newSoItem;
            amount = 0;
            SetAmount(newAmount);
        }
        
        public int SetItem(ItemAmount itemAmount)
        {
            if (itemAmount == null || itemAmount.IsEmpty)
            {
                Clear();
                return 0;
            }

            soItem = itemAmount.SoItem;
            return SetAmount(itemAmount.Amount); 
        }

        public void SetItem(SoItem newItem)
        {
            soItem = newItem;
            
            if (soItem == null)
            {
                amount = 0;
                return;
            }
            
            if (amount > Stack)
                amount = Stack;
        }
        
        public int SetAmount(int newAmount)
        {
            if (!ValidSoItem)
            {
                Clear();
                return newAmount;
            }

            int clampedAmount = Mathf.Clamp(newAmount, 0, Stack);

            amount = clampedAmount;

            if (amount <= 0)
                Clear();

            // The remainder of what didnt fit in the stack
            return newAmount - clampedAmount; 
        }
        
        public int AddAmount(int amountToAdd)
        {
            if (IsEmpty || amountToAdd <= 0) return amountToAdd;
            return SetAmount(amount + amountToAdd);
        }
        
        public int RemoveAmount(int amountToRemove)
        {
            if (IsEmpty || amountToRemove <= 0) return amountToRemove;
            
            int removable = Mathf.Min(amount, amountToRemove);
            amount -= removable;

            if (amount <= 0)
                Clear();

            int remainingToRemove = amountToRemove - removable;
            return remainingToRemove;
        }

        public void Clear()
        {
            soItem = null;
            amount = 0;
        }
    }
}
