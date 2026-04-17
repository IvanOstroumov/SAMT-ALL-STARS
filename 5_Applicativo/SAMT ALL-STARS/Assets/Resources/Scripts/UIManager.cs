using UnityEngine;
using UnityEngine.SceneManagement;

namespace Resources.Scripts
{
    public class UIManager:MonoBehaviour
    {
        public static void openMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public static void openCharacterselect()
        {
            SceneManager.LoadScene("CharacterSelection");
        }

        public static void openMapSelect()
        {
            SceneManager.LoadScene("MapSelection");
        }

        public static void openGame()
        {
            SceneManager.LoadScene("Game");
        }

        public static void openSettings()
        {
            SceneManager.LoadScene("Settings");
        }

        public static void openWiki()
        {
            SceneManager.LoadScene("Wiki");
        }

        public static void openResults()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public static void quit()
        {
            #if UnityEditor
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}