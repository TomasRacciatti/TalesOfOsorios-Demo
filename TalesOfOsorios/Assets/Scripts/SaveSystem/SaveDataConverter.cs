using Items.Core;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public static class SaveDataConverter
    {
        // Take InvSystem and convert to data
        public static InventorySaveData ConvertToSaveData(InvSystem invSystem)
        {
            if (invSystem == null)
            {
                return new InventorySaveData();
            }

            InventorySaveData saveData = new InventorySaveData(invSystem.Size);

            for (int i = 0; i < invSystem.Items.Count; i++)
            {
                ItemAmount itemAmount = invSystem.Items[i];

                if (itemAmount == null || itemAmount.IsEmpty)
                {
                    saveData.items[i] = new SerializableItemData();
                }
                else
                {
                    saveData.items[i] = new SerializableItemData(
                        itemAmount.SoItem.name,
                        itemAmount.Amount
                    );
                }
            }

            return saveData;
        }

        // Take inv data and convert it to the inv system
        public static void LoadIntoInventory(InventorySaveData saveData, InvSystem invSystem)
        {
            if (saveData == null || invSystem == null)
            {
                Debug.LogWarning("Cannot load inventory: null data or system");
                return;
            }

            for (int i = 0; i < saveData.items.Count && i < invSystem.Size; i++)
            {
                SerializableItemData itemData = saveData.items[i];

                if (itemData == null || itemData.IsEmpty)
                {
                    invSystem.SetItemByIndex(i, new ItemAmount());
                    continue;
                }

                SoItem soItem = LoadItemByName(itemData.itemName);

                if (soItem != null)
                {
                    ItemAmount itemAmount = new ItemAmount(soItem, itemData.amount);
                    invSystem.SetItemByIndex(i, itemAmount);
                }
                else
                {
                    Debug.LogWarning($"Could not find item: {itemData.itemName}");
                    invSystem.SetItemByIndex(i, new ItemAmount());
                }
            }
        }

        // Finds SO name through Resources.LoadAll of type SoItem
        private static SoItem LoadItemByName(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
                return null;

            SoItem[] allItems = Resources.LoadAll<SoItem>("Items");

            foreach (SoItem item in allItems)
            {
                if (item.name == itemName)
                {
                    return item;
                }
            }
            return null;
        }
    }
}