using Items.Inventory;
using UnityEngine;

namespace Managers
{
    public class InvManager : MonoBehaviour
    {
        [Header("Inventory Systems")]
        [SerializeField] private InvSystem baseInventory;
        [SerializeField] private InvSystem equipInventory;
    
        [Header("UI")]
        [SerializeField] private Animator animator;
    
        private bool _inventoryOpen = false;
        
        public InvSystem BaseInventory => baseInventory;
        public InvSystem EquipInventory => equipInventory;
        public bool IsOpen => _inventoryOpen;
    
        private void Awake()
        {
            if (animator == null) animator = GetComponent<Animator>();
        }
    
        private void Start()
        {
            _inventoryOpen = false;
            animator.Play("CloseInventory", 0, 1f);
            Cursor.visible = false;
        }
    
        public void ForceInventory(bool open)
        {
            if (_inventoryOpen == open) return;
            ToggleInventory();
        }

        public bool ToggleInventory()
        {
            _inventoryOpen = !_inventoryOpen;
            animator.speed = 1;
            animator.Play(_inventoryOpen ? "OpenInventory" : "CloseInventory", 0, 
                1 - Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
        
            if (!_inventoryOpen)
            {
                ItemsTooltip.Hide();
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
            return _inventoryOpen;
        }
    }
}
