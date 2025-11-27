using System;
using System.Collections.Generic;
using Items.Core;
using UnityEngine;

public class InvSystem : MonoBehaviour
{
    [SerializeField] private int size = 10;
    [SerializeField] private bool allowStacking = true;
    [SerializeField] private bool canGrab = true;

    [SerializeField] private List<ItemAmount> items = new List<ItemAmount>();
    
    public Func<SoItem, bool> AcceptRule;

    public int Size => size;
    public bool AllowStacking => allowStacking;
    public bool CanGrab => canGrab;

    public IReadOnlyList<ItemAmount> Items => items;

    public event Action<int, ItemAmount> OnItemChanged;

    private void Awake()
    {
        items ??= new List<ItemAmount>();
        while (items.Count < size)
            items.Add(new ItemAmount());
    }
    
    public bool AddItem(ref ItemAmount newItem)
    {
        if (newItem == null || newItem.IsEmpty)
            return false;
        
        if (AcceptRule != null && !AcceptRule(newItem.SoItem))
            return false;

        bool addedAnything = false;


        if (allowStacking)
        {
            for (int i = 0; i < size && !newItem.IsEmpty; i++)
            {
                ItemAmount slot = items[i];

                if (slot.IsEmpty) 
                    continue;

                if (ItemsUtility.Stackable(slot, newItem))
                {
                    int remainder = slot.AddAmount(newItem.Amount);
                    newItem.SetAmount(remainder);

                    OnItemChanged?.Invoke(i, slot);

                    if (remainder < newItem.Amount)
                        addedAnything = true;
                }
            }
        }
        
        for (int i = 0; i < size && !newItem.IsEmpty; i++)
        {
            ItemAmount slot = items[i];

            if (slot.IsEmpty)
            {
                int remainder = newItem.SetItem(slot);
                items[i] = new ItemAmount(newItem.SoItem, newItem.Amount);

                OnItemChanged?.Invoke(i, items[i]);

                if (items[i].Amount > 0)
                    addedAnything = true;

                newItem.SetAmount(remainder);
            }
        }

        return addedAnything;
    }
    
    public bool RemoveItem(SoItem soItem, int amount)
    {
        if (soItem == null || amount <= 0) return false;

        int remaining = amount;

        for (int i = 0; i < size && remaining > 0; i++)
        {
            ItemAmount slot = items[i];

            if (slot.SoItem != soItem)
                continue;

            int before = remaining;
            remaining = slot.RemoveAmount(remaining);

            OnItemChanged?.Invoke(i, slot);
            
            if (slot.IsEmpty)
                items[i] = new ItemAmount();
        }

        return remaining == 0;
    }
    
    public void RemoveItem(ref ItemAmount itemAmount)
    {
        if (itemAmount == null || itemAmount.IsEmpty) return;

        int remaining = itemAmount.Amount;

        for (int i = 0; i < size && remaining > 0; i++)
        {
            ItemAmount slot = items[i];

            if (slot.IsEmpty || slot.SoItem != itemAmount.SoItem) 
                continue;

            int before = remaining;
            remaining = slot.RemoveAmount(remaining);

            OnItemChanged?.Invoke(i, slot);

            if (slot.IsEmpty)
                items[i] = new ItemAmount();
        }
        
        itemAmount.SetAmount(remaining);
    }
    
    public void Swap(int indexA, int indexB)
    {
        if (indexA == indexB) return;
        if (indexA < 0 || indexA >= size) return;
        if (indexB < 0 || indexB >= size) return;

        (items[indexA], items[indexB]) = (items[indexB], items[indexA]);

        OnItemChanged?.Invoke(indexA, items[indexA]);
        OnItemChanged?.Invoke(indexB, items[indexB]);
    }
    
    public void TransferIndexToIndex(InvSystem other, int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex && this == other) return;

        if (fromIndex < 0 || fromIndex >= size) return;
        if (toIndex < 0 || toIndex >= other.size) return;

        ItemAmount from = items[fromIndex];
        if (from.IsEmpty) return;
        
        if (other.AcceptRule != null && !other.AcceptRule(from.SoItem))
            return;

        ItemAmount to = other.items[toIndex];

        bool canStack =
            other.allowStacking &&
            !to.IsEmpty &&
            ItemsUtility.Stackable(to, from);

        if (canStack)
        {
            int remainder = to.AddAmount(from.Amount);
            from.SetAmount(remainder);

            other.OnItemChanged?.Invoke(toIndex, to);
            OnItemChanged?.Invoke(fromIndex, from);

            if (from.IsEmpty)
                items[fromIndex] = new ItemAmount();

            return;
        }

        // Swap if can't place
        Swap(toIndex, fromIndex);
    }
}
