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
        [SerializeField, Min(1)] private int stack = 10;
        
        [Header("Usage / Equipment")] 
        [SerializeField] private bool isConsumable; 
        [SerializeField] private bool isEquipable;
        [SerializeField] private string equipSlotId;

        public string ItemName => itemName;
        public Sprite Icon => icon;
        public string Description => description;
        public int Stack => stack;
        public bool IsConsumable => isConsumable;
        public bool IsEquipable => isEquipable;
        public string EquipSlotId => equipSlotId;
    }
}