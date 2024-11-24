using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    private readonly WaitForSeconds _wait = new WaitForSeconds(7f);
    
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
    [SerializeField] private CanvasGroup newChat;
    [SerializeField] private CanvasGroup credit;
    [SerializeField] public GUI.CreditUI creditUI;
    [SerializeField] private Button storeButton;

    private void Awake()
    {
        DontDestroyOnLoad(Instance);
        SceneManager.sceneLoaded += ToggleUI;
    }

    private void Start()
    {
        ShowUI(mainMenu);
        HideUI(sideMenu);
        HideUI(chat);
        HideUI(newChat);
        HideUI(joystick);
        HideUI(credit);
    }
    
    private void ToggleUI(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case SceneName.Login:
                ShowUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                HideUI(newChat);
                HideUI(credit);
                storeButton.interactable = false;
                break;
            case SceneName.Square:
                HideUI(newChat);
                TemporarilyHideUI();
                storeButton.interactable = true;
                break;
            case SceneName.MyRoom:
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                screenshotButton.interactable = false;
                ShowUI(chat);   
                ShowUI(joystick);
                HideUI(newChat);
                HideUI(credit);
                storeButton.interactable = true;
                break;
            case SceneName.FindPhoto:
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                ShowUI(chat);
                ShowUI(joystick);
                ShowUI(credit);
                storeButton.interactable = false;
                break;
            case SceneName.MapEdit:
                ShowUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);   
                HideUI(newChat);
                HideUI(credit);
                break;
            case SceneName.PlayReady:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                HideUI(newChat);
                HideUI(credit);
                storeButton.interactable = false;
                break;
            case SceneName.SelectPhotoScene:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
                storeButton.interactable = false;
                break;  
            case SceneName.VideoLoadTest:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
                storeButton.interactable = false;
                break;
            case SceneName.LetterScene:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
                storeButton.interactable = false;
                break;
            default:
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                ShowUI(chat);
                ShowUI(joystick);
                HideUI(newChat);
                ShowUI(credit);
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

    private void TemporarilyHideUI()
    {
        if (SceneTracker.PreviousScene == SceneName.Login)
        {
            StartCoroutine(ProcessTemporaryToggle());
        }
    }

    private IEnumerator ProcessTemporaryToggle()
    {
        HideUI(mainMenu);
        HideUI(sideMenu);
        HideUI(chat);
        HideUI(joystick);
        HideUI(credit);
        yield return _wait;
        ShowUI(mainMenu);
        ShowUI(sideMenu);
        ShowUI(chat);
        ShowUI(joystick);
        ShowUI(credit);
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}