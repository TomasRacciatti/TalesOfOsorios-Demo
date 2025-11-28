using System;
using UnityEngine;

namespace Items.Inventory
{
    public class EquipInventoryBinder : MonoBehaviour
    {
        [SerializeField] private InvSystem equipSystem;
        [SerializeField] private EquipSlotId slotId = EquipSlotId.None;

        private void Awake()
        {
            equipSystem.AcceptRule = (soItem) =>
            {
                if (soItem == null)
                    return false;

                return soItem.EquipSlotId == slotId;
            };
        }
    }

    public enum EquipSlotId
    {
        None,
        Potion,
        Chest,
        Waist,
        Arms,
        Hands,
        Shoulder,
        Legs
    }
}
