using UnityEngine;

namespace Managers
{
    public class CanvasManager : MonoBehaviour
    {
        private static CanvasManager _instance;
        
        [SerializeField] private InvManager invManager;
        [SerializeField] private MenuManager pauseMenuController;
        [SerializeField] private MenuManager gameOverMenuController;
        
        [SerializeField] private GameObject saveText;
        
        public InvManager InvManager => invManager;
        public GameObject SaveText => saveText;
        
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
        
         public bool TogglePauseMenu()
         {
             if (pauseMenuController != null)
             {
                 pauseMenuController.TogglePause();
                
                 return pauseMenuController.IsOpen;
             }
             return false;
         }

         public void ShowGameOver()
         {
             if (gameOverMenuController != null)
             {
                 gameOverMenuController.ShowGameOver();
             }
         }
        
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
