using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Button))]
public class MemoryGem : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        var yggdrasil = FindAnyObjectByType<ToggleYggdrasil>();
        _button.onClick.AddListener(yggdrasil.DisplayContentUi);
        _button.onClick.AddListener(Test);
    }

    private void Test()
    {
        Debug.Log("?????");
    }
    
    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}