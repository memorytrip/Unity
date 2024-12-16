using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    private readonly WaitForSeconds _wait = new WaitForSeconds(5f);
    
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
    [SerializeField] private Button walletButton;
    [SerializeField] private Button exitAppButton;
    [SerializeField] private CanvasGroup myRoomButton;
    

    private void Awake()
    {
        DontDestroyOnLoad(Instance);
        SceneManager.sceneLoaded += ToggleUI;
        exitAppButton.onClick.AddListener(ExitApp);
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

    public void HideMenu()
    {
        HideUI(sideMenu);
        HideUI(chat);
    }

    public void ShowMenu()
    {
        ShowUI(sideMenu);
        ShowUI(chat);
    }
    
    private void ToggleUI(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"UIManager.ToggleUI: {scene.name}");
        switch (scene.name)
        {
            case SceneName.EmptyScene:
                return;
            case SceneName.Login:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                HideUI(newChat);
                HideUI(credit);
                storeButton.interactable = false;
                walletButton.interactable = false;
                break;
            case SceneName.Square:
                HideUI(newChat);
                TemporarilyHideUI();
                screenshotButton.interactable = true;
                storeButton.interactable = true;
                walletButton.interactable = true;
                myRoomButton.interactable = true;
                break;
            case SceneName.MyRoom:
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                screenshotButton.interactable = false;
                ShowUI(chat);   
                ShowUI(joystick);
                HideUI(newChat);
                HideUI(credit);
				HideUI(myRoomButton);
                storeButton.interactable = true;
                myRoomButton.interactable = false;
                break;
            case SceneName.FindPhoto:
                ShowUI(mainMenu);
                ShowUI(sideMenu);
                ShowUI(chat);
                ShowUI(joystick);
                ShowUI(credit);
				HideUI(myRoomButton);
                storeButton.interactable = false;
                myRoomButton.interactable = false;
                screenshotButton.interactable = false;
                break;
            case SceneName.MapEdit:
                ShowUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);   
                HideUI(newChat);
                HideUI(credit);
				HideUI(myRoomButton);
                break;
            case SceneName.PlayReady:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                HideUI(newChat);
                HideUI(credit);
				HideUI(myRoomButton);
                storeButton.interactable = false;
                break;
            case SceneName.SelectPhotoScene:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
				HideUI(myRoomButton);
                storeButton.interactable = false;
                break;  
            case SceneName.VideoLoadTest:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
				HideUI(myRoomButton);
                storeButton.interactable = false;
                break;
            case SceneName.LetterScene:
                HideUI(mainMenu);
                HideUI(sideMenu);
                HideUI(chat);
                HideUI(joystick);
                ShowUI(newChat);
                HideUI(credit);
				HideUI(myRoomButton);
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
        Debug.Log($"UIManager.TemporarilyHideUI: {SceneTracker.PreviousScene}");
        if (SceneTracker.PreviousScene == SceneName.Login)
        {
            StartCoroutine(ProcessTemporaryToggle());
        }
        else
        {
            ShowUI(mainMenu);
            ShowUI(sideMenu);
            ShowUI(chat);
            ShowUI(joystick);
            ShowUI(credit);
			ShowUI(myRoomButton);
        }
    }

    private IEnumerator ProcessTemporaryToggle()
    {
        HideUI(mainMenu);
        HideUI(sideMenu);
        HideUI(chat);
        HideUI(joystick);
        HideUI(credit);
        HideUI(myRoomButton);
        yield return _wait;
        ShowUI(mainMenu);
        ShowUI(sideMenu);
        ShowUI(chat);
        ShowUI(joystick);
        ShowUI(credit);
        ShowUI(myRoomButton);
        InitMyroomButton();
    }

    private void InitMyroomButton() {
        myRoomButton.GetComponent<Button>().onClick.RemoveAllListeners();
        myRoomButton.GetComponent<Button>().onClick.AddListener(() => {
            GameObject obj = GameObject.Find("Teleport_Popup");
            if ( obj != null )
            {
                ShowUI(obj.GetComponent<CanvasGroup>());
            }
        });
    }

    private void ExitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ToggleUI;
    }
}