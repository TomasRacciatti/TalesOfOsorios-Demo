using System;

namespace SaveSystem
{
    [Serializable]
    public class SerializableItemData
    {
        public string itemName;
        public int amount;

        public SerializableItemData()
        {
            itemName = string.Empty;
            amount = 0;
        }
        
        public SerializableItemData(string itemName, int amount)
        {
            this.itemName = itemName;
            this.amount = amount;
        }
        
        public bool IsEmpty => string.IsNullOrEmpty(itemName) || amount <= 0;
    }
}
