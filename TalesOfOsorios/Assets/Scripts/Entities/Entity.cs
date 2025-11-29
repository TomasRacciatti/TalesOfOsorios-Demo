using UnityEngine;
using System;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        [SerializeField] protected EntityStats stats;
        
        [Header("Combat")]
        [SerializeField] protected WeaponCollider weaponCollider;
        [SerializeField] protected float heavyAttackMultiplier = 1.5f;
        
        [Header("Animation")]
        [SerializeField] protected Animator animator;
    
        protected float currentHealth;
        protected float currentSpeed;
        protected bool isDead = false;
        protected bool isAttacking = false;
        protected float lastAttackTime = -1f;
    
        public float CurrentHealth => currentHealth;
        public float CurrentSpeed => currentSpeed;
        public bool IsDead => isDead;
        public EntityStats Stats => stats;
        public float MaxHealth => stats.MaxHealth;
        public bool IsAttacking => isAttacking;
    
        public event Action<float> OnHealthChanged;
        public event Action<float> OnDamageReceived;
        
        private const string ANIM_IS_MOVING = "IsMoving";
        private const string ANIM_ATTACK = "Attack";
        private const string ANIM_HEAVY_ATTACK = "HeavyAttack";
        private const string ANIM_HIT = "Hit";
        private const string ANIM_DIE = "Die";
        private const string ANIM_DEAD = "Dead";

        protected virtual void Awake()
        {
            if (stats == null)
            {
                Debug.LogError($"EntityStats missing on {gameObject.name}");
                return;
            }
            
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            
            if (weaponCollider == null)
                weaponCollider = GetComponentInChildren<WeaponCollider>();
        
            currentHealth = stats.MaxHealth;
            currentSpeed = stats.BaseSpeed;
        }

        public virtual void TakeDamage(float damage)
        {
            if (isDead) return;
            
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0f);
            
            OnHealthChanged?.Invoke(currentHealth);
            OnDamageReceived?.Invoke(damage);
            
            HandleDamageEffect(damage); 

            if (currentHealth <= 0)
                Die();
        }

        public virtual void Heal(float healValue)
        {
            if (isDead) return;
            
            currentHealth += healValue;
            currentHealth = Mathf.Min(currentHealth, stats.MaxHealth);
            
            OnHealthChanged?.Invoke(currentHealth);
        }

        public virtual void Die()
        {
            if (isDead) return;
            
            isDead = true;
            HandleDeath();
        }
        
        public void UpdateMovementAnimation(float speed)
        {
            if (animator == null || isDead) return;
            
            bool isMoving = Mathf.Abs(speed) > 0.01f;
            animator.SetBool(ANIM_IS_MOVING, isMoving);
        }
        
        public void PerformAttack()
        {
            if (isAttacking || isDead || Time.time < lastAttackTime + stats.AttackCooldown) return;
        
            isAttacking = true;
            lastAttackTime = Time.time;
            animator.SetTrigger(ANIM_ATTACK);
        }
    
        public void PerformHeavyAttack()
        {
            if (isAttacking || isDead || Time.time < lastAttackTime + stats.AttackCooldown) return;
        
            isAttacking = true;
            lastAttackTime = Time.time;
            animator.SetTrigger(ANIM_HEAVY_ATTACK);
        }
        
        public void OnAttackStart()
        {
            if (weaponCollider == null) return;
        
            float damage = GetRandomAttackDamage();
            weaponCollider.EnableCollider(damage);
        }
    
        public void OnHeavyAttackStart()
        {
            if (weaponCollider == null) return;
        
            float damage = GetRandomAttackDamage() * heavyAttackMultiplier;
            weaponCollider.EnableCollider(damage);
        }
    
        public void OnAttackEnd()
        {
            if (weaponCollider == null) return;
        
            weaponCollider.DisableCollider();
        }
    
        public void OnAttackAnimationEnd()
        {
            isAttacking = false;
        }
        
        protected virtual void HandleDamageEffect(float damage)
        {
            if (animator != null && !isDead)
            {
                animator.SetTrigger(ANIM_HIT);
            }
        }
        
        protected virtual void HandleDeath()
        {
            if (animator != null)
            {
                animator.SetBool(ANIM_DEAD, true);
                animator.SetTrigger(ANIM_DIE);
            }
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
    }
}
