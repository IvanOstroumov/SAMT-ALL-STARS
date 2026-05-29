using UnityEngine;
using UnityEngine.SceneManagement;

namespace Resources.Scripts
{
    // Tutti i passaggi di scena del gioco in un posto solo.
    // I bottoni della UI agganciano questi metodi statici via OnClick.
    // Centralizzare qui evita di sparpagliare LoadScene("nomeScritto male") in giro.
    public class UIManager : MonoBehaviour
    {
        public static void openMainMenu()
        {
            LogManager.Info("Apertura MainMenu");
            SceneManager.LoadScene("MainMenu");
        }

        public static void openCharacterselect()
        {
            LogManager.Info("Apertura CharacterSelection");
            SceneManager.LoadScene("CharacterSelection");
        }

        public static void openMapSelect()
        {
            LogManager.Info("Apertura MapSelection");
            SceneManager.LoadScene("MapSelection");
        }

        public static void openGame()
        {
            LogManager.Info("Apertura Game");
            SceneManager.LoadScene("Game");
        }

        public static void openSettings()
        {
            LogManager.Info("Apertura Settings");
            SceneManager.LoadScene("Settings");
        }

        public static void openWiki()
        {
            LogManager.Info("Apertura Wiki");
            SceneManager.LoadScene("Wiki");
        }

        public static void openResults()
        {
            // Per ora rimanda al menu, in attesa di una scena Results vera.
            SceneManager.LoadScene("MainMenu");
        }

        public static void openPostMatch()
        {
            SceneManager.LoadScene("PostMatch");
        }
        public static void quit()
        {
            LogManager.Info("Quit richiesto");

            // In editor non si puo' chiudere l'applicazione vera: spengo il Play.
            // Nella build vera (player standalone) chiudo il processo.
            #if UnityEditor
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
