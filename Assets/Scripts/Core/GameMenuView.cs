using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameMenuView : MonoBehaviour
    {
        public void Menu()
        {
            Debug.LogError("Main Menu");
            GameManager.Instance.BackgroundAudio.Stop();
            SceneManager.LoadSceneAsync("MainMenu");
            GameManager.Instance.IsGameOver = true;
        }

        public void Exit()
        {
            Debug.LogError("Exit");
            Application.Quit();
        }
    }
}
