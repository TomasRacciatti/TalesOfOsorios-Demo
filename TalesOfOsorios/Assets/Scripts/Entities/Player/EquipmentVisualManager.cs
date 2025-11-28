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

        private Dictionary<EquipSlotId, List<GameObject>> _slotVisualsMap;
        private InvSystem _equipInventoryCache;
        
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

            _equipInventoryCache = equipInventory;
            equipInventory.OnItemChanged += OnEquipmentChanged;

            RefreshAllEquipmentVisuals();
        }
        
        private void InitializeSlotVisualsMap()
        {
            _slotVisualsMap = new Dictionary<EquipSlotId, List<GameObject>>();

            foreach (var slotVisual in equipmentSlots)
            {
                if (!_slotVisualsMap.ContainsKey(slotVisual.slotId))
                {
                    _slotVisualsMap[slotVisual.slotId] = new List<GameObject>();
                }

                _slotVisualsMap[slotVisual.slotId].AddRange(slotVisual.visualObjects);
            }
        }

        private void OnEquipmentChanged(int slotIndex, ItemAmount itemAmount)
        {
            if (itemAmount == null || itemAmount.IsEmpty)
            {
                RefreshAllEquipmentVisuals();
                return;
            }

            EquipSlotId slotId = itemAmount.SoItem.EquipSlotId;

            if (slotId == EquipSlotId.None || slotId == EquipSlotId.Potion)
            {
                return;
            }

            UpdateSlotVisuals(slotId, true);
        }
        
        private void UpdateSlotVisuals(EquipSlotId slotId, bool isEquipped)
        {
            if (!_slotVisualsMap.ContainsKey(slotId))
            {
                return;
            }

            foreach (var visualObject in _slotVisualsMap[slotId])
            {
                if (visualObject != null)
                {
                    visualObject.SetActive(isEquipped);
                }
            }
        }
        
        private void RefreshAllEquipmentVisuals()
        {
            foreach (var slotVisuals in _slotVisualsMap.Values)
            {
                foreach (var visualObject in slotVisuals)
                {
                    if (visualObject != null)
                    {
                        visualObject.SetActive(false);
                    }
                }
            }

            InvSystem equipInv = GetEquipInventory();
            if (equipInv != null)
            {
                for (int i = 0; i < equipInv.Items.Count; i++)
                {
                    ItemAmount item = equipInv.Items[i];
                    if (item != null && !item.IsEmpty)
                    {
                        EquipSlotId slotId = item.SoItem.EquipSlotId;
                        if (slotId != EquipSlotId.None && slotId != EquipSlotId.Potion)
                        {
                            UpdateSlotVisuals(slotId, true);
                        }
                    }
                }
            }
        }
        
        private InvSystem GetEquipInventory()
        {
            if (_equipInventoryCache != null) return _equipInventoryCache;
            
            if (Managers.GameManager.Canvas != null)
            {
                _equipInventoryCache = Managers.GameManager.Canvas.InvManager?.EquipInventory;
            }
            
            return _equipInventoryCache;
        }
        
        
        [System.Serializable]
        public class EquipmentSlotVisuals
        {
            public EquipSlotId slotId;
            public List<GameObject> visualObjects = new List<GameObject>();
        }
    }
}