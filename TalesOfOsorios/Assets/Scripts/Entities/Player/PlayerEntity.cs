using System;
using UnityEngine;
using Items.Core;

namespace Entities.Player
{
    public class PlayerEntity : Entity
    {
        public event Action OnPlayerDeath;

        private float _totalArmor = 0f;
        private InvSystem _equipInventory;
        
        public float TotalArmor => _totalArmor;

        private void Start()
        {
            InitializeArmorTracking();
        }
        
        private void InitializeArmorTracking()
        {
            _equipInventory = Managers.GameManager.Canvas.InvManager.EquipInventory;
            
            if (_equipInventory != null)
            {
                _equipInventory.OnItemChanged += OnEquipmentChanged;
                RecalculateArmor();
            }
        }

        private void OnEquipmentChanged(int slotIndex, ItemAmount itemAmount)
        {
            RecalculateArmor();
        }

        private void RecalculateArmor()
        {
            _totalArmor = 0f;

            if (_equipInventory == null) return;

            foreach (var item in _equipInventory.Items)
            {
                if (item != null && !item.IsEmpty && item.SoItem is SoEquipment equipment)
                {
                    _totalArmor += equipment.Armor;
                }
            }

            //Debug.Log($"Total Armor recalculated: {_totalArmor}");
        }

        protected override void HandleDamageEffect(float damage)
        {
            base.HandleDamageEffect(damage);
        
            Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{MaxHealth}");
        }
        
        protected override void HandleDeath()
        {
            base.HandleDeath();
            
            OnPlayerDeath?.Invoke();
        }

        public override void TakeDamage(float damage)
        {
            if (isDead) return;
            
            float damageAfterArmor = Mathf.Max(5, damage - _totalArmor);
    
            base.TakeDamage(damageAfterArmor);
            
            Debug.Log($"Player took {damage} damage, reduced by {_totalArmor} armor. Final damage: {damageAfterArmor}. Health: {currentHealth}/{MaxHealth}");
        }
    }
}
