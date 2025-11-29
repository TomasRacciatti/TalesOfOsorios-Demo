using System;
using Managers;
using UnityEngine;
using Entities.Player;

namespace Entities.Enemy
{
    [RequireComponent(typeof(Rigidbody2D), typeof(EnemyEntity))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Patrol Settings")]
        [SerializeField] private Transform patrolPointA;
        [SerializeField] private Transform patrolPointB;
        [SerializeField] private float patrolWaitTime = 2f;

        [Header("Detection Settings")]
        [SerializeField] private float detectionRange = 8f;
        [SerializeField] private float obligatoryChaseRange = 4f;
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private LayerMask playerLayer;

        [Header("Visual")]
        [SerializeField] private Transform visualTransform;
        
        // VERY basic AI, not ideal to the simple FSM I would have wanted,
        // but something to prototype due to time constraints
        private enum EnemyState { Patrolling, Chasing, Attacking }
        private EnemyState _currentState = EnemyState.Patrolling;

        private Rigidbody2D _rb;
        private EnemyEntity _enemyEntity;
        private PlayerEntity _playerEntity;
        private Transform _playerTransform;
        private Transform _currentPatrolTarget;
        private float _patrolWaitTimer;
        private bool _isFacingRight = false;
        
        private const float PATROL_REACH_THRESHOLD = 0.5f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _enemyEntity = GetComponent<EnemyEntity>();

            if (patrolPointA != null)
            {
                _currentPatrolTarget = patrolPointA;
            }
            
            if (visualTransform != null && !_isFacingRight)
            {
                Flip();
            }
        }

        private void Start()
        {
            _playerTransform = GameManager.Player.transform;
            
            _playerEntity = GameManager.Player.GetComponent<PlayerEntity>();
            if (_playerEntity != null)
            {
                _playerEntity.OnPlayerDeath += OnPlayerDeath;
            }
        }
        
        private void OnPlayerDeath()
        {
            _currentState = EnemyState.Patrolling;
            _rb.linearVelocity = Vector2.zero;
        }
        
        private void OnDestroy()
        {
            if (GameManager.Player != null)
            {
                if (_playerEntity != null)
                {
                    _playerEntity.OnPlayerDeath -= OnPlayerDeath;
                }
            }
        }

        private void Update()
        {
            if (_enemyEntity.IsDead || _enemyEntity.IsAttacking)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            UpdateState();
            UpdateBehavior();
            UpdateFlip();
            UpdateAnimation();
        }
        
        // Again this is very basic and not how I would Ideally do this,
        // but I needed to prioritize speed over better code
        private void UpdateState() 
        {
            if (_playerTransform == null) return;
            
            if (_playerEntity != null && _playerEntity.IsDead)
            {
                _currentState = EnemyState.Patrolling;
                return;
            }

            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            bool facingPlayer = IsFacingPlayer();

            if (distanceToPlayer <= attackRange)
            {
                _currentState = EnemyState.Attacking;
            }
            else if (distanceToPlayer <  obligatoryChaseRange || distanceToPlayer <= detectionRange && facingPlayer)
            {
                _currentState = EnemyState.Chasing;
            }
            else
            {
                _currentState = EnemyState.Patrolling;
            }
        }

        private bool IsFacingPlayer()
        {
            if (_playerTransform == null) return false;
    
            var directionToPlayer = _playerTransform.position.x - transform.position.x;
    
            if (_isFacingRight)
            {
                return directionToPlayer > 0;
            }
            else
            {
                return directionToPlayer < 0;
            }
        }
        
        private void UpdateBehavior()
        {
            switch (_currentState)
            {
                case EnemyState.Patrolling:
                    Patrol();
                    break;
                case EnemyState.Chasing:
                    ChasePlayer();
                    break;
                case EnemyState.Attacking:
                    AttackPlayer();
                    break;
            }
        }
        
        private void Patrol()
        {
            if (patrolPointA == null || patrolPointB == null)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            if (_patrolWaitTimer > 0f)
            {
                _patrolWaitTimer -= Time.deltaTime;
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            var distanceToTarget = Vector2.Distance(transform.position, _currentPatrolTarget.position);

            if (distanceToTarget < PATROL_REACH_THRESHOLD)
            {
                _currentPatrolTarget = _currentPatrolTarget == patrolPointA ? patrolPointB : patrolPointA;
                _patrolWaitTimer = patrolWaitTime;
                _rb.linearVelocity = Vector2.zero;
            }
            else
            {
                MoveTowards(_currentPatrolTarget.position);
            }
        }
        
        private void ChasePlayer()
        {
            if (_playerTransform != null)
            {
                MoveTowards(_playerTransform.position);
            }
        }
        
        private void AttackPlayer()
        {
            _rb.linearVelocity = Vector2.zero;

            if (!_enemyEntity.IsAttacking)
            {
                _enemyEntity.PerformAttack();
            }
        }
        
        private void MoveTowards(Vector3 targetPosition)
        {
            float direction = Mathf.Sign(targetPosition.x - transform.position.x);
            _rb.linearVelocity = new Vector2(direction * _enemyEntity.CurrentSpeed, _rb.linearVelocity.y);
        }

        private void UpdateFlip()
        {
            if (visualTransform == null) return;

            float velocityX = _rb.linearVelocity.x;

            if (velocityX > 0.01f && !_isFacingRight)
            {
                Flip();
            }
            else if (velocityX < -0.01f && _isFacingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            _isFacingRight = !_isFacingRight;

            Vector3 scale = visualTransform.localScale;
            scale.x *= -1;
            visualTransform.localScale = scale;
        }
        
        private void UpdateAnimation()
        {
            float speed = Mathf.Abs(_rb.linearVelocity.x);
            _enemyEntity.UpdateMovementAnimation(speed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, obligatoryChaseRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
