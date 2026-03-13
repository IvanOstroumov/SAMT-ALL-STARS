using UnityEngine;

namespace Resources.Scripts
{
    public static class UIManager
    {
        public static void openMainMenu()
        {
        }

        public static void openCharacterselect()
        {
        }

        public static void openMapSelect()
        {
        }

        public static void openGame()
        {
        }

        public static void openSettings()
        {
        }

        public static void openWiki()
        {
        }

        public static void openResults()
        {
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