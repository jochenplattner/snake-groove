using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnSettings()
    {
        Debug.Log("[MainMenu] Settings clicked");
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
