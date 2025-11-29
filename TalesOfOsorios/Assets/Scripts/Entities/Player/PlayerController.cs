using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Entities.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PlayerEntity), typeof(InteractionDetector))]
    public class PlayerController : MonoBehaviour, PlayerInput.IPlayerMappingActions
    {
        [Header("References")]
        [SerializeField] private PlayerEntity playerEntity;
        [SerializeField] private Transform visualTransform;
        
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private bool _isFacingRight = false;
        
        private PlayerInput _playerInput;
        private bool _inputEnabled = true;

        private InteractionDetector _interactionDetector;
        

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _playerInput.PlayerMapping.SetCallbacks(this);
            
            if (playerEntity == null)
            {
                playerEntity = GetComponent<PlayerEntity>();
            }
            
            _rb = GetComponent<Rigidbody2D>();
            _interactionDetector = GetComponent<InteractionDetector>();
            
            if (visualTransform != null && !_isFacingRight)
            {
                Flip();
            }
        }

        private void Start()
        {
            GameManager.RegisterPlayer(this);
            Instantiate(PrefabsManager.Canvas, null, false);

            if (SaveSystem.SaveSystem.ShouldLoadOnStart)
            {
                StartCoroutine(LoadGameAfterCanvasReady());
            }
            else
            {
                GameManager.Resume();
            }
        }

        private IEnumerator LoadGameAfterCanvasReady()
        {
            yield return new WaitForEndOfFrame();
            
            if (GameManager.Canvas != null && GameManager.Canvas.InvManager != null)
            {
                SaveSystem.GameSaveData loadedData = SaveSystem.SaveSystem.LoadGame();
                
                SaveSystem.SaveDataConverter.LoadIntoInventory(
                    loadedData.baseInventory, GameManager.Canvas.InvManager.BaseInventory);
                
                SaveSystem.SaveDataConverter.LoadIntoInventory(
                    loadedData.equipInventory, GameManager.Canvas.InvManager.EquipInventory);
            }
            
            SaveSystem.SaveSystem.ShouldLoadOnStart = false;
            GameManager.Resume();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
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

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed && GameManager.Canvas != null)
            {
                GameManager.Canvas.TogglePauseMenu();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!_inputEnabled) return;
            
            float horizontalInput = context.ReadValue<float>();
            _moveInput = new Vector2(horizontalInput, 0f);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed && !playerEntity.IsDead && _inputEnabled)
            {
                playerEntity.PerformAttack();
            }
        }

        public void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (context.performed && !playerEntity.IsDead && _inputEnabled)
            {
                playerEntity.PerformHeavyAttack();
            }
        }

        public void OnUsePotion(InputAction.CallbackContext context)
        {
            // TODO: Potion logic
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed && !playerEntity.IsDead && _interactionDetector != null && _inputEnabled)
            {
                _interactionDetector.TryInteract();
            }
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            if (context.performed && GameManager.Canvas != null)
            {
                bool isOpen = GameManager.Canvas.ToggleInventory();
            }
        }

        private void HandleMovement()
        {
            float targetVelocityX = _moveInput.x * playerEntity.CurrentSpeed;
            _rb.linearVelocity = new Vector2(targetVelocityX, _rb.linearVelocity.y);
        }

        private void HandleFlip()
        {
            if (visualTransform == null) return;
            
            if (_moveInput.x > 0 && !_isFacingRight)
            {
                Flip();
            }
            else if (_moveInput.x < 0 && _isFacingRight)
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

        private void UpdateAnimations()
        {
            playerEntity.UpdateMovementAnimation(_moveInput.x);
        }
        
        public void DisableGameplayInput()
        {
            _inputEnabled = false;
            _moveInput = Vector2.zero;
        }
        
        public void EnableGameplayInput()
        {
            _inputEnabled = true;
        }
    }
}
