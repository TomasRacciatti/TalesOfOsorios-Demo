using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class InventorySaveData
    {
        public List<SerializableItemData> items;
        
        public InventorySaveData()
        {
            items = new List<SerializableItemData>();
        }

        public InventorySaveData(int size)
        {
            items = new List<SerializableItemData>(size);
            for (int i = 0; i < size; i++)
            {
                items.Add(new SerializableItemData());
            }
        }
    }
}
