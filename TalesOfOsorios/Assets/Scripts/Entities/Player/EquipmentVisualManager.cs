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
        [SerializeField] private List<GameObject> equipmentVisuals;
        
        private InvSystem _invSystem;
        private List<SpriteRenderer> _spriteRenderers;
        
        private void Awake()
        {
            _spriteRenderers = new List<SpriteRenderer>();

            foreach (var go in equipmentVisuals)
            {
                if (go == null) continue;
                
                var render = go.GetComponent<SpriteRenderer>();
                
                if (render != null) _spriteRenderers.Add(render);
            }
        }

        private void Start()
        {
            StartCoroutine(InitializeEquipmentSystem());
        }


        private void SetSprite(int index, ItemAmount itemAmount)
        {
            if (index < 0 || index >= equipmentVisuals.Count) return;
            
            var visualObject = equipmentVisuals[index];
            var render = _spriteRenderers[index];

            if (itemAmount.IsEmpty)
            {
                visualObject.SetActive(false);
                return;
            }
            
            visualObject.SetActive(true);
            
            if (itemAmount.SoItem.Icon != null)
            {
                render.sprite = itemAmount.SoItem.Icon;
            }
        }
        
        private IEnumerator InitializeEquipmentSystem()
        {
            while (_invSystem == null)
            {
                if (Managers.GameManager.Canvas != null && 
                    Managers.GameManager.Canvas.InvManager != null)
                {
                    _invSystem = Managers.GameManager.Canvas.InvManager.EquipInventory;
                    
                    if (_invSystem != null)
                    {
                        _invSystem.OnItemChanged += SetSprite;
                        yield break;
                    }
                }
                yield return null;
            }
        }
    }
}