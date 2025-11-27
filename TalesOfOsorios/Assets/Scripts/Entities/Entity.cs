using UnityEngine;
using System;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        [SerializeField] protected EntityStats stats;
    
        protected float currentHealth;
        protected float currentSpeed;
        protected Rigidbody2D rb;
        protected bool isDead = false;
    
        public float CurrentHealth => currentHealth;
        public float CurrentSpeed => currentSpeed;
        public bool IsDead => isDead;
        public EntityStats Stats => stats;
        public float MaxHealth => stats.MaxHealth;
    
        public event Action<float> OnHealthChanged;
        public event Action<float> OnDamageReceived;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        
            if (stats == null)
            {
                Debug.LogError($"EntityStats missing on {gameObject.name}");
                return;
            }
        
            currentHealth = stats.MaxHealth;
            currentSpeed = stats.BaseSpeed;
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;
            
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, stats.MaxHealth);
            
            OnHealthChanged?.Invoke(currentHealth);
            OnDamageReceived?.Invoke(damage);

            if (currentHealth <= 0)
                Die();
        }

        public virtual void Heal(float healValue)
        {
            if (isDead) return;
            
            currentHealth += healValue;
            currentHealth = Mathf.Clamp(currentHealth, 0, stats.MaxHealth);
            
            OnHealthChanged?.Invoke(currentHealth);
        }

        public virtual void Die()
        {
            isDead = true;
            HandleDeath();
        }
        
        protected void ModifySpeed(float speedMultiplier)
        {
            currentSpeed = stats.BaseSpeed * speedMultiplier;
        }
        
        protected void ReturnSpeed()
        {
            currentSpeed = stats.BaseSpeed;
        }
        
        protected float GetRandomAttackDamage()
        {
            return UnityEngine.Random.Range(stats.MinDamage, stats.MaxDamage);
        }
        
        protected abstract void HandleDamageEffect(float damage);
        protected abstract void HandleDeath();
    }
}
