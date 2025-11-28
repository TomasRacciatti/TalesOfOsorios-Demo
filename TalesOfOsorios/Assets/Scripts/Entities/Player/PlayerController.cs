using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerEntity))]
    public class PlayerController : MonoBehaviour, PlayerInput.IPlayerMappingActions
    {
        [Header("References")]
        [SerializeField] private PlayerEntity playerEntity;
        [SerializeField] private Transform visualTransform;
        
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isFacingRight = false;
        
        private PlayerInput playerInput;
        

        private void Awake()
        {
            playerInput = new PlayerInput();
            playerInput.PlayerMapping.SetCallbacks(this);
            
            if (playerEntity == null)
            {
                playerEntity = GetComponent<PlayerEntity>();
            }
            
            rb = GetComponent<Rigidbody2D>();
            
            if (visualTransform != null && !isFacingRight)
            {
                Flip();
            }
        }

        private void Start()
        {
            GameManager.Resume();
            GameManager.RegisterPlayer(this);
            Instantiate(PrefabsManager.Canvas, null, false);
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }

        private void OnDisable()
        {
            playerInput.Disable();
        }

        private void FixedUpdate()
        {
            if (playerEntity.IsDead) return;
            
            HandleMovement();
        }

        private void Update()
        {
            if (playerEntity.IsDead) return;
            
            HandleFlip();
            UpdateAnimations();
        }

        public void OnClimb(InputAction.CallbackContext context)
        {
            // TODO: Climbing logic
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            float horizontalInput = context.ReadValue<float>();
            moveInput = new Vector2(horizontalInput, 0f);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed && !playerEntity.IsDead)
            {
                playerEntity.PerformAttack();
            }
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (context.performed && !playerEntity.IsDead)
            {
                playerEntity.PerformHeavyAttack();
            }
        }

        public void OnUsePotion(InputAction.CallbackContext context)
        {
            // TODO: Potion logic
        }
        
        private void HandleMovement()
        {
            float targetVelocityX = moveInput.x * playerEntity.CurrentSpeed;
            rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
        }

        private void HandleFlip()
        {
            if (visualTransform == null) return;
            
            if (moveInput.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveInput.x < 0 && isFacingRight)
            {
                Flip();
            }
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            
            Vector3 scale = visualTransform.localScale;
            scale.x *= -1;
            visualTransform.localScale = scale;
        }

        private void UpdateAnimations()
        {
            playerEntity.UpdateMovementAnimation(moveInput.x);
        }
    }
}
