using UnityEngine;
using UnityEngine.SceneManagement;
using SaveSystem;
using System.Collections;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Pause Panels")]
        [SerializeField] private GameObject panel;
        [SerializeField] private GameObject bg;
        
        private bool _isOpen;
        private bool _isGameOver;
        
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
        
        public void ShowGameOver()
        {
            if (panel != null && bg != null)
            {
                panel.SetActive(true);
                bg.SetActive(true);
                _isGameOver = true;
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
            
            saveData.worldItems = SaveDataConverter.ConvertWorldItemsToSaveData();
            
            SaveSystem.SaveSystem.SaveGame(saveData);

            StartCoroutine(ActivateSaveText(2f));
        }

        private IEnumerator ActivateSaveText(float duration)
        {
            GameManager.Canvas.SaveText.SetActive(true);
            yield return new WaitForSeconds(duration);
            GameManager.Canvas.SaveText.SetActive(false);
        }

        public void LoadGame()
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
