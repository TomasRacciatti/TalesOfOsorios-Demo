using UnityEngine;
using UnityEngine.SceneManagement;

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
            //ToDo: implement save logic
        }

        public void LoadGame()
        {
            //ToDo: implement load logic
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
