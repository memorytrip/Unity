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
    [SerializeField] private CanvasGroup credit;
    [SerializeField] public GUI.CreditUI creditUI;

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
            case "Login":
                ShowUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                HideUI(newChat);
                HideUI(credit);
                break;
            case "SpecialSquare":
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
                HideUI(credit);
                break;
            case "FindPhoto":
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                ShowUI(chat);
                ShowUI(joystick);
                ShowUI(credit);
                break;
            case "MapEdit":
                ShowUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);   
                HideUI(newChat);
                HideUI(credit);
                break;
            case "PlayReady":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                HideUI(newChat);
                HideUI(credit);
                break;
            case "SelectPhotoScene":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
                break;  
            case "VideoLoadTest":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
                break;
            case "LetterScene":
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
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