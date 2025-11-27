using System;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerEntity : Entity
    {
        public event Action OnPlayerDeath;
        
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
    }
}
