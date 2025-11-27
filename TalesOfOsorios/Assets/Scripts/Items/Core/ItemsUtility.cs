namespace Items.Core
{
    public class ItemsUtility
    {
        public static bool Stackable(ItemAmount itemA, ItemAmount itemB)
        {
            if (itemA == null || itemB == null) return false;
            if (itemA.IsEmpty || itemB.IsEmpty) return false;
            
            return itemA.SoItem == itemB.SoItem;
        }
    }
}
