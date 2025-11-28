using UnityEngine;
using Items.Inventory;

public class InvManager : MonoBehaviour
{
    [Header("Inventory Systems")]
    [SerializeField] private InvSystem baseInventory;
    [SerializeField] private InvSystem equipInventory;
    
    [Header("UI")]
    [SerializeField] private Animator animator;
    
    private bool inventoryOpen = false;
        
    public InvSystem BaseInventory => baseInventory;
    public InvSystem EquipInventory => equipInventory;
    public bool IsOpen => inventoryOpen;
    
    private void Awake()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        inventoryOpen = false;
        animator.Play("CloseInventory", 0, 1f);
        Cursor.visible = false;
    }
    
    public void ForceInventory(bool open)
    {
        if (inventoryOpen == open) return;
        ToggleInventory();
    }

    public bool ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        animator.speed = 1;
        animator.Play(inventoryOpen ? "OpenInventory" : "CloseInventory", 0, 
            1 - Mathf.Clamp01(animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
        
        if (!inventoryOpen)
        {
            ItemsTooltip.Hide();
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
        return inventoryOpen;
    }
}
