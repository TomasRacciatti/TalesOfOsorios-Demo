using UnityEngine;
using UnityEngine.SceneManagement;
using SaveSystem;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Pause Panels")]
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject bg;
        private bool _isOpen;
        
        public bool IsOpen => _isOpen;
        
        private void Start()
        {
            _isOpen = false;
        }
        
        public void TogglePause()
        {
            if (_isOpen)
            {
                GameManager.Resume();
                GameManager.Player.EnableGameplayInput();

                bg.SetActive(false);
                panel.SetActive(false);
                _isOpen = false;
            }
            else
            {
                GameManager.Player.DisableGameplayInput(); 
                bg.SetActive(true);
                panel.SetActive(true);
                _isOpen = true;
            
                GameManager.Pause();
            }
        }
        
        public void PlayGame()
        {
            GameManager.Resume();
            SceneManager.LoadScene("Game");
        }
        
        public void ToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void SaveGame()
        {
            if (GameManager.Canvas == null || GameManager.Canvas.InvManager == null)
            {
                return;
            }
            
            GameSaveData saveData = new GameSaveData();

            saveData.baseInventory = SaveDataConverter.ConvertToSaveData(
                GameManager.Canvas.InvManager.BaseInventory);
            
            saveData.equipInventory = SaveDataConverter.ConvertToSaveData(
                GameManager.Canvas.InvManager.EquipInventory);
            
            SaveSystem.SaveSystem.SaveGame(saveData);
        }

        public void LoadGame()
        {
            bool isInGameScene = SceneManager.GetActiveScene().name == "Game";

            if (isInGameScene)
            {
                LoadGameIngame();
            }
            else
            {
                LoadGameFromMainMenu();
            }
        }

        private void LoadGameIngame()
        {
            if (GameManager.Canvas == null || GameManager.Canvas.InvManager == null)
            {
                Debug.LogError("Cannot load: InvManager not found!");
                return;
            }
            
            GameSaveData loadedData = SaveSystem.SaveSystem.LoadGame();

            SaveDataConverter.LoadIntoInventory(
                loadedData.baseInventory, GameManager.Canvas.InvManager.BaseInventory);

            SaveDataConverter.LoadIntoInventory(
                loadedData.equipInventory, GameManager.Canvas.InvManager.EquipInventory);
            
            Debug.Log("Game loaded");
        }

        private void LoadGameFromMainMenu()
        {
            if (!SaveSystem.SaveSystem.SaveExists())
            {
                Debug.LogWarning("No save file found!");
                return;
            }

            SaveSystem.SaveSystem.ShouldLoadOnStart = true;
            SceneManager.LoadScene("Game");
        }
        
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
