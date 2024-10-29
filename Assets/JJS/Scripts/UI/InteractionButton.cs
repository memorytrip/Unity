using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour, IAnimate
{
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    private Animator _animator;
    [SerializeField] private Button button;
    [SerializeField] private Button screenshotModeButton;
    [SerializeField] private Button interactButton;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ToggleBool);
        screenshotModeButton.onClick.AddListener(ToggleBool);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ToggleBool);
        screenshotModeButton.onClick.RemoveListener(ToggleBool);
    }
    
    public void SetBool(int cachedProperty, bool value)
    {
        _animator.SetBool(cachedProperty, value);
    }

    public void ToggleBool()
    {
        SetBool(IsOpen, !_animator.GetBool(IsOpen));
    }

    public void SetTrigger(int cachedProperty)
    {
        _animator.SetTrigger(cachedProperty);
    }
}
