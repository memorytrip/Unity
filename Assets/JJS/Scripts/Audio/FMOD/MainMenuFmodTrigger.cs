using UnityEngine;
using UnityEngine.UI;

public class MainMenuFmodTrigger : MonoBehaviour
{
    private Button _mainMenuButton;
    [SerializeField] private Button cancelMainMenuButton;
    
    private FmodGlobalParameterSetter fmodGlobalParameterSetter;

    private void Awake()
    {
        _mainMenuButton = GetComponent<Button>();
        fmodGlobalParameterSetter = FindAnyObjectByType<FmodGlobalParameterSetter>();
    }

    private void Start()
    {
        _mainMenuButton.onClick.AddListener(fmodGlobalParameterSetter.TogglePause);
        cancelMainMenuButton.onClick.AddListener(fmodGlobalParameterSetter.TogglePause);
        cancelMainMenuButton.onClick.AddListener(What);
    }

    private void What()
    {
        Debug.Log("WHAAAAAAT");
    }
    

    private void OnDestroy()
    {
        _mainMenuButton.onClick.RemoveAllListeners();
    }
}