using UnityEngine;
using UnityEngine.SceneManagement;

namespace SnakeGroove.UI.Screens
{
    public sealed class MainMenuScreen : UIScreen
    {
        public void OnStartClicked()
        {
            Debug.Log("[MainMenu] Start clicked");
            SceneManager.LoadScene("Game");
        }

        public void OnSettingsClicked()
        {
            Debug.Log("[MainMenu] Settings clicked");
        }

        public void OnExitClicked()
        {
            Debug.Log("[MainMenu] Exit clicked");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}


