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
            case "PlayReady":
            case "MapEdit":
                ToggleOn(mainMenu);
                ToggleOff(sideMenu);
                ToggleOff(chat);
                ToggleOff(joystick);
                break;
            default:
                ToggleOn(mainMenu);
                ToggleOn(sideMenu);
                ToggleOn(chat);
                ToggleOn(joystick);
                break;
        }
    }

    public static void ToggleOn(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
    }

    public static void ToggleOff(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}