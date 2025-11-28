using System;
using System.Collections;
using System.Collections.Generic;
using Items.Inventory;
using UnityEngine;
using Items.Core;

namespace Entities.Player
{
    public class EquipmentVisualManager : MonoBehaviour
    {
        [SerializeField, Tooltip("0: Legs, 1: Waist, 2: Chest, 3; Shoulder, 4: Arms, 5: Hand")]
        private List<EquipmentSlot> equipmentVisuals;

        private InvSystem _invSystem;
        private List<SpriteRenderer[]> _spriteRenderers;

        private void Awake()
        {
            _spriteRenderers = new List<SpriteRenderer[]>();

            foreach (var slot in equipmentVisuals)
            {
                if (slot == null || slot.visualObjects == null) continue;

                SpriteRenderer[] renderers = new SpriteRenderer[slot.visualObjects.Length];

                for (int i = 0; i < slot.visualObjects.Length; i++)
                {
                    if (slot.visualObjects[i] != null)
                    {
                        renderers[i] = slot.visualObjects[i].GetComponent<SpriteRenderer>();
                    }
                }

                _spriteRenderers.Add(renderers);
            }
        }

        private void Start()
        {
            InitializeEquipmentSystem();
        }


        private void SetSprite(int index, ItemAmount itemAmount)
        {
            if (index < 0 || index >= equipmentVisuals.Count) return;

            var renderers = _spriteRenderers[index];
            var visualObjects = equipmentVisuals[index].visualObjects;

            if (itemAmount.IsEmpty)
            {
                foreach (var go in visualObjects)
                {
                    if (go != null) go.SetActive(false);
                }

                return;
            }

            for (int i = 0; i < visualObjects.Length; i++)
            {
                visualObjects[i].SetActive(true);

                if (i == 0 && itemAmount.SoItem.Icon != null)
                {
                    renderers[i].sprite = itemAmount.SoItem.Icon;
                }
                else if (i == 1 && itemAmount.SoItem.IconSecond != null)
                {
                    renderers[i].sprite = itemAmount.SoItem.IconSecond;
                }
            }
        }

        private void InitializeEquipmentSystem()
        {
            if (Managers.GameManager.Canvas != null &&
                Managers.GameManager.Canvas.InvManager != null)
            {
                _invSystem = Managers.GameManager.Canvas.InvManager.EquipInventory;

                if (_invSystem != null)
                {
                    _invSystem.OnItemChanged += SetSprite;
                }
            }
        }

        [System.Serializable]
        public class EquipmentSlot
        {
            public GameObject[] visualObjects = new GameObject[1];
        }
    }
}