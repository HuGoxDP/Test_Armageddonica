using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Architecture.Core
{
    public class GameMenu : MonoBehaviour
    {
        public void LoadGame()
        {
            SceneManager.LoadScene("Game");
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}