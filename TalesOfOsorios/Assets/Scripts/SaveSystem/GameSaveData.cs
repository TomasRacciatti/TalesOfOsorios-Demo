using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class GameSaveData
    {
        public int gameStartCount;
        public InventorySaveData baseInventory;
        public InventorySaveData equipInventory;
        public string saveDateTime;
        public List<WorldItemData> worldItems;

        public GameSaveData()
        {
            gameStartCount = 0;
            baseInventory = new InventorySaveData();
            equipInventory = new InventorySaveData();
            saveDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            worldItems = new List<WorldItemData>();
        }
    }
}
