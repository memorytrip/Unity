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
                mainMenu.alpha = 1f;
                mainMenu.interactable = mainMenu.blocksRaycasts = true;
                sideMenu.alpha = 0f;
                sideMenu.interactable = sideMenu.blocksRaycasts = false;
                chat.alpha = 0f;
                chat.interactable = chat.blocksRaycasts = false;
                joystick.alpha = 0f;
                joystick.interactable = joystick.blocksRaycasts = false;
                break;
            default:
                mainMenu.alpha = 1f;
                mainMenu.interactable = mainMenu.blocksRaycasts = true;
                sideMenu.alpha = 1f;
                sideMenu.interactable = sideMenu.blocksRaycasts = true;
                chat.alpha = 1f;
                chat.interactable = chat.blocksRaycasts = true;
                joystick.alpha = 1f;
                joystick.interactable = joystick.blocksRaycasts = true;
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}