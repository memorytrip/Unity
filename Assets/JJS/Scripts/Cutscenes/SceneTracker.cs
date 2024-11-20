using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static string PreviousScene { get; private set; } = string.Empty;
    private const string LoadingSceneName = "EmptyScene";
    private string _currentActiveScene = string.Empty;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == LoadingSceneName)
        {
            return;
        }
        
        var activeScene = SceneManager.GetActiveScene();
        if (_currentActiveScene != activeScene.name)
        {
            PreviousScene = _currentActiveScene;
            _currentActiveScene = activeScene.name;
        }
    }
}