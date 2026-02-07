using UnityEngine;

namespace SnakeGroove.Infrastructure
{
    /// <summary>
    /// Singleton GameManager to manage game state and persist across scenes.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Debug.Log("[GameManager] Instance created: " + GetInstanceID());
        }
    }
}
