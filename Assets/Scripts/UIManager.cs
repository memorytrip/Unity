using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField] private CanvasGroup mainMenu;
    [SerializeField] private CanvasGroup sideMenu;
    [SerializeField] private Button screenshotButton;
    [SerializeField] private CanvasGroup chat;
    [SerializeField] private CanvasGroup joystick;

    private void Awake()
    {
        DontDestroyOnLoad(Instance);
        SceneManager.sceneLoaded += ToggleUI;
    }
    
    private void ToggleUI(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Login":
                ShowUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                break;
            case "MyRoom":
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                screenshotButton.interactable = false;
                HideUI(chat);
                ShowUI(joystick);
                break;
            case "MapEdit":
            case "PlayReady":
            case "SelectPhotoScene":
            case "VideoLoadTest":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                break;
            default:
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                ShowUI(chat);
                ShowUI(joystick);
                break;
        }
    }

    public static void ShowUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
    }

    public static void HideUI(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}