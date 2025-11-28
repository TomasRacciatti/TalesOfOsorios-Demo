using Items.Inventory;
using UnityEngine;

namespace Items.Core
{
    [CreateAssetMenu(fileName = "SoItem", menuName = "Scriptable Objects/Items/SoItem")]
    public class SoItem : ScriptableObject
    {
        [Header("Item")] 
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;
        [SerializeField, TextArea] private string description = "Item Description";
        [SerializeField, TextArea] private string summary = "Heals 20 pts";
        [SerializeField, Min(1)] private int stack = 5;
        
        [Header("Usage / Equipment")]
        [SerializeField] private EquipSlotId equipSlotId;

        public string ItemName => itemName;
        public Sprite Icon => icon;
        public string Description => description;
        public int Stack => stack;
        public EquipSlotId EquipSlotId => equipSlotId;
    }
}