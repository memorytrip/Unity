using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
                HideUI(newChat);
                break;
            case "Square":
                HideUI(newChat);
                TemporarilyHideUI();
                break;
            case "MyRoom":
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                screenshotButton.interactable = false;
                ShowUI(chat);   
                ShowUI(joystick);
                HideUI(newChat);
                break;
            case "MapEdit":
                ShowUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);   
                HideUI(newChat);
                break;
            case "PlayReady":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                HideUI(newChat);
                break;
            case "SelectPhotoScene":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                break;  
            case "VideoLoadTest":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                break;
            case "LetterScene":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                break;
            default:
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                ShowUI(chat);
                ShowUI(joystick);
                HideUI(newChat);
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

    public void TemporarilyHideUI()
    {
        StartCoroutine(ProcessTemporaryToggle());
    }

    private IEnumerator ProcessTemporaryToggle()
    {
        HideUI(mainMenu);
        HideUI(sideMenu);
        HideUI(chat);
        HideUI(joystick);
        yield return _wait;
        ShowUI(mainMenu);
        ShowUI(sideMenu);
        ShowUI(chat);
        ShowUI(joystick);
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}