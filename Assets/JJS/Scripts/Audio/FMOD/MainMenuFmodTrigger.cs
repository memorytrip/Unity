using UnityEngine;
using UnityEngine.UI;

public class MainMenuFmodTrigger : MonoBehaviour
{
    private Button _mainMenuButton;
    [SerializeField] private Button cancelMainMenuButton;
    
    private FmodParameterSetter _fmodParameterSetter;

    private void Awake()
    {
        _mainMenuButton = GetComponent<Button>();
        _fmodParameterSetter = FindAnyObjectByType<FmodParameterSetter>();
    }

    private void Start()
    {
        _mainMenuButton.onClick.AddListener(_fmodParameterSetter.TogglePause);
        cancelMainMenuButton.onClick.AddListener(_fmodParameterSetter.TogglePause);
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