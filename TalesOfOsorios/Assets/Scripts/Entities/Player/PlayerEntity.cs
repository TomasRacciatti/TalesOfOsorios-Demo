using System;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerEntity : Entity
    {
        [Header("Combat")]
        [SerializeField] private float heavyAttackMultiplier = 1.5f;
        [SerializeField] private WeaponCollider weaponCollider;
    
        [Header("Animation")]
        [SerializeField] private Animator animator;
    
        private bool isAttacking;
    
        private const string ANIM_SPEED = "Speed";
        private const string ANIM_ATTACK = "Attack";
        private const string ANIM_HEAVY_ATTACK = "HeavyAttack";
        private const string ANIM_HIT = "Hit";
        private const string ANIM_DIE = "Die";
    
        public bool IsAttacking => isAttacking;

        public event Action OnPlayerDeath;

        protected override void Awake()
        {
            base.Awake();

            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
                
                if (animator == null)
                    Debug.LogError($"No Animator found on {gameObject.name} or its children");
            }

            if (weaponCollider == null)
            {
                weaponCollider = GetComponentInChildren<WeaponCollider>();
                
                if (weaponCollider == null)
                    Debug.LogError($"No WeaponCollider found on {gameObject.name} or its children");
            }
        }

        public void UpdateMovementAnimation(float speed)
        {
            if (animator == null || isDead) return;
            
            animator.SetFloat(ANIM_SPEED, Mathf.Abs(speed));
        }
        
        public void PerformAttack()
        {
            if (isAttacking || isDead) return;
        
            isAttacking = true;
            animator.SetTrigger(ANIM_ATTACK);
        }
    
        public void PerformHeavyAttack()
        {
            if (isAttacking || isDead) return;
        
            isAttacking = true;
            animator.SetTrigger(ANIM_HEAVY_ATTACK);
        }
    
        public void OnAttackStart()
        {
            float damage = GetRandomAttackDamage();
            weaponCollider.EnableCollider(damage);
        }
    
        public void OnHeavyAttackStart()
        {
            float damage = GetRandomAttackDamage() * heavyAttackMultiplier;
            weaponCollider.EnableCollider(damage);
        }
    
        public void OnAttackEnd()
        {
            weaponCollider.DisableCollider();
        }
        
        public void OnAttackAnimationEnd()
        {
            isAttacking = false;
        }
        
        protected override void HandleDamageEffect(float damage)
        {
            if (animator != null && !isDead)
                animator.SetTrigger(ANIM_HIT);
        
            Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{MaxHealth}");
        }

        protected override void HandleDeath()
        {
            if (animator != null)
                animator.SetTrigger(ANIM_DIE);
            
        
            OnPlayerDeath?.Invoke();
            
            Debug.Log($"Player died");
        }
    }
}
