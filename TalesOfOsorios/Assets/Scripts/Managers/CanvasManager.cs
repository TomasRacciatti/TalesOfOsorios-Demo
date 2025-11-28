using UnityEngine;

namespace Managers
{
    public class CanvasManager : MonoBehaviour
    {
        private static CanvasManager _instance;
        
        [SerializeField] private InvManager invManager;
        
        public InvManager InvManager => invManager;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(_instance.gameObject);
            }
            _instance = this;
            
            invManager.gameObject.SetActive(true);
            
            GameManager.RegisterCanvas(this);
        }
        
        //ToDo: Implement pause logic
        // public bool TogglePauseMenu()
        // {
        //     pauseMenuUI.TogglePause();
        //     return pauseMenuUI.Open;
        // }
        
        public bool ToggleInventory()
        {
            bool isOpen = invManager.ToggleInventory();

            if (isOpen)
            {
                GameManager.Player.DisableGameplayInput();
            }
            else
            {
                GameManager.Player.EnableGameplayInput();
            }
            
            return isOpen;
        }
    }
}
