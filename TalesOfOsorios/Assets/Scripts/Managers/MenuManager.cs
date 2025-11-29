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

                bg.SetActive(false);
                panel.SetActive(false);
                _isOpen = false;
            }
            else
            {
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
            GameManager.Resume();
            SceneManager.LoadScene("MainMenu");
        }

        public void SaveGame()
        {
            if (GameManager.Canvas == null || GameManager.Canvas.InvManager == null)
            {
                Debug.LogError("Cannot save: InvManager not found!");
                return;
            }
            
            GameSaveData saveData = new GameSaveData();

            saveData.baseInventory = SaveDataConverter.ConvertToSaveData(
                GameManager.Canvas.InvManager.BaseInventory);
            
            saveData.equipInventory = SaveDataConverter.ConvertToSaveData(
                GameManager.Canvas.InvManager.EquipInventory);
            
            SaveSystem.SaveSystem.SaveGame(saveData);
            Debug.Log("Game saved");
        }

        public void LoadGame()
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
