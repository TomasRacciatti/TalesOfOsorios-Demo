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
                //Debug.LogWarning("Cannot load inventory: null data or system");
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

        public static List<WorldItemData> ConvertWorldItemsToSaveData()
        {
            List<WorldItemData> worldItemsData = new List<WorldItemData>();
            
            ItemPickup[] allPickups = Object.FindObjectsOfType<ItemPickup>();
            
            foreach (ItemPickup pickup in allPickups)
            {
                if (pickup.ItemAmount == null || pickup.ItemAmount.IsEmpty)
                    continue;
                    
                WorldItemData itemData = new WorldItemData(
                    pickup.ItemAmount.SoItem.name,
                    pickup.ItemAmount.Amount,
                    pickup.transform.position
                );
                
                worldItemsData.Add(itemData);
            }
            
            return worldItemsData;
        }

        public static void LoadWorldItems(List<WorldItemData> worldItemsData, GameObject itemPickupPrefab)
        {
            if (itemPickupPrefab == null)
            {
                Debug.LogError("Cannot load world items: itemPickupPrefab is null!");
                return;
            }

            // Clear all items
            ItemPickup[] existingPickups = Object.FindObjectsOfType<ItemPickup>();
            foreach (ItemPickup pickup in existingPickups)
            {
                Object.Destroy(pickup.gameObject);
            }

            // All items picked up = no data (so we leave it empty)
            if (worldItemsData == null || worldItemsData.Count == 0)
            {
                return;
            }

            // We spawn saved items
            foreach (WorldItemData itemData in worldItemsData)
            {
                if (itemData == null || itemData.IsEmpty)
                    continue;

                var soItem = LoadItemByName(itemData.itemName);
                
                if (soItem != null)
                {
                    var position = new Vector3(itemData.positionX, itemData.positionY, itemData.positionZ);
                    var itemObject = Object.Instantiate(itemPickupPrefab, position, Quaternion.identity);
                    
                    var pickup = itemObject.GetComponent<ItemPickup>();
                    if (pickup != null)
                    {
                        ItemAmount itemAmount = new ItemAmount(soItem, itemData.amount);
                        pickup.SetItemAmount(itemAmount);
                    }
                }
                else
                {
                    Debug.LogWarning($"Could not find item for world spawn: {itemData.itemName}");
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