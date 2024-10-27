using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup sideMenu;
    [SerializeField] private CanvasGroup chat;
    [SerializeField] private CanvasGroup joystick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        SceneManager.sceneLoaded += ToggleUI;
    }
    
    private void ToggleUI(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Login":
                mainMenu.alpha = 1f;
                Debug.Log($"MainMenu alpha: {mainMenu.alpha}");
                sideMenu.alpha = 0f;
                Debug.Log($"SideMenu alpha: {sideMenu.alpha}");
                chat.alpha = 0f;
                Debug.Log($"Chat alpha: {chat.alpha}");
                joystick.alpha = 0f;
                Debug.Log($"Joystick alpha: {joystick.alpha}");
                break;
            default:
                mainMenu.alpha = 1f;
                sideMenu.alpha = 1f;
                chat.alpha = 1f;
                joystick.alpha = 1f;
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}