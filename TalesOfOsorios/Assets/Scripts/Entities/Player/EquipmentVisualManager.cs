using System.Collections.Generic;
using Items.Inventory;
using UnityEngine;
using Items.Core;

namespace Entities.Player
{
    public class EquipmentVisualManager : MonoBehaviour
    {
        [Header("Equipment Visuals Mapping")]
        [SerializeField] private List<EquipmentSlotVisuals> equipmentSlots = new List<EquipmentSlotVisuals>();

        private Dictionary<EquipSlotId, List<GameObject>> slotVisualsMap;
        
        private void Awake()
        {
            InitializeSlotVisualsMap();
        }
        
        public void Initialize(InvSystem equipInventory)
        {
            if (equipInventory == null)
            {
                return;
            }

            equipInventory.OnItemChanged += OnEquipmentChanged;

            for (int i = 0; i < equipInventory.Items.Count; i++)
            {
                OnEquipmentChanged(i, equipInventory.Items[i]);
            }
        }
        
        private void InitializeSlotVisualsMap()
        {
            slotVisualsMap = new Dictionary<EquipSlotId, List<GameObject>>();

            foreach (var slotVisual in equipmentSlots)
            {
                if (!slotVisualsMap.ContainsKey(slotVisual.slotId))
                {
                    slotVisualsMap[slotVisual.slotId] = new List<GameObject>();
                }

                slotVisualsMap[slotVisual.slotId].AddRange(slotVisual.visualObjects);
            }
        }

        private void OnEquipmentChanged(int slotIndex, ItemAmount itemAmount)
        {

            EquipSlotId slotId = EquipSlotId.None;

            if (itemAmount != null && !itemAmount.IsEmpty)
            {
                slotId = itemAmount.SoItem.EquipSlotId;
        
                if (slotId != EquipSlotId.None && slotId != EquipSlotId.Potion)
                {
                    UpdateSlotVisuals(slotId, true);
                }
            }
        }
        
        private void UpdateSlotVisuals(EquipSlotId slotId, bool isEquipped)
        {
            if (!slotVisualsMap.ContainsKey(slotId))
            {
                return;
            }

            foreach (var visualObject in slotVisualsMap[slotId])
            {
                if (visualObject != null)
                {
                    visualObject.SetActive(isEquipped);
                }
            }
        }

        private void DisableAllEquipmentVisuals()
        {
            foreach (var slotVisuals in slotVisualsMap.Values)
            {
                foreach (var visualObject in slotVisuals)
                {
                    if (visualObject != null)
                    {
                        visualObject.SetActive(false);
                    }
                }
            }
        }
        
        public void UnequipSlot(EquipSlotId slotId)
        {
            UpdateSlotVisuals(slotId, false);
        }
        
        
        [System.Serializable]
        public class EquipmentSlotVisuals
        {
            public EquipSlotId slotId;
            public List<GameObject> visualObjects = new List<GameObject>();
        }
    }

}