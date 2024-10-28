using UnityEngine;
using UnityEngine.SceneManagement;

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
                ShowUi(mainMenu);
                HideUi(sideMenu);
                HideUi(chat);
                HideUi(joystick);
                break;
            default:
                ShowUi(mainMenu);
                ShowUi(sideMenu);
                ShowUi(chat);
                ShowUi(joystick);
                break;
        }
    }

    private void ShowUi(CanvasGroup target)
    {
        target.alpha = 1f;
        Debug.Log($"WHY {target} {target.alpha}");
        target.interactable = true;
        target.blocksRaycasts = true;
    }

    private void HideUi(CanvasGroup target)
    {
        target.alpha = 0f;
        Debug.Log($"Why alpha no change: {target} {target.alpha}");
        target.interactable = false;
        target.blocksRaycasts = false;
    }

    public void SwitchUI()
    {
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}