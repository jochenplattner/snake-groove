using UnityEngine;
using UnityEngine.SceneManagement;

namespace SnakeGroove.Core
{
    public class BootLoader : MonoBehaviour
    {
        [SerializeField]
        private string _nextSceneName = "MainMenu";

        private void Start()
        {
            Debug.Log("[BootLoader] Loading scene: " + _nextSceneName);
            SceneManager.LoadScene(_nextSceneName);
        }
    }
}
