using System;

namespace SaveSystem
{
    [Serializable]
    public class WorldItemData
    {
        public string itemName;
        public int amount;
        public float positionX;
        public float positionY;
        public float positionZ;

        public WorldItemData()
        {
            itemName = string.Empty;
            amount = 0;
            positionX = 0f;
            positionY = 0f;
            positionZ = 0f;
        }

        public WorldItemData(string name, int amt, UnityEngine.Vector3 pos, bool initial)
        {
            itemName = name;
            amount = amt;
            positionX = pos.x;
            positionY = pos.y;
            positionZ = pos.z;
        }

        public bool IsEmpty => string.IsNullOrEmpty(itemName) || amount <= 0;
    }
}
