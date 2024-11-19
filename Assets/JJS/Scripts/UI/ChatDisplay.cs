using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class ChatDisplay : MonoBehaviour
{
    public enum ChatDisplayMode
    {
        Shrink,
        Expand,
    }
    
    public bool IsTyping => _inputField != null && _inputField.isFocused;
    
    public event Action<bool> ChangeTypingState;

    public void OnTypingStateChanged(bool typingStatus)
    {
        ChangeTypingState?.Invoke(typingStatus);
    }
    
    public event Action<bool> StopTyping;

    public void OnStoppedTyping(bool typingStatus)
    {
        StopTyping?.Invoke(typingStatus);
    }
    
    public CanvasGroup chatDisplay;
    public CanvasGroup chatScroll;
    private TMP_InputField _inputField;
    [SerializeField] private Button expandChatButton;
    private ChatDisplayMode _chatDisplayMode = ChatDisplayMode.Shrink;
    private const float MinHeight = 100f;
    private const float MaxHeight = 400f;
    
    private void Awake()
    {
        //_chatDisplayArea = GetComponent<RectTransform>();
        //_chatDisplayArea.sizeDelta = new Vector2(_chatDisplayArea.sizeDelta.x, MinHeight);
        
        _inputField = GetComponentInChildren<TMP_InputField>();
        
        expandChatButton.onClick.AddListener(ToggleChat);

        if (_inputField != null)
        {
            _inputField.restoreOriginalTextOnEscape = true;
            _inputField.onSelect.AddListener(_ => StartChat());
            _inputField.onDeselect.AddListener(_ => StopChat());
            _inputField.onSubmit.AddListener(SubmitChat);
        }
    }

    private void StartChat()
    {
        if (!IsTyping)
        {
            OnTypingStateChanged(true);
        }
    }

    private void StopChat()
    {
        if (IsTyping)
        {
            OnTypingStateChanged(false);
        }
    }

    private void SubmitChat(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }
        _inputField.text = string.Empty;
        StopChat();
    }

    private void ToggleChat()
    {
        switch (_chatDisplayMode)
        {
            case ChatDisplayMode.Shrink:
                ExpandChat();
                break;
            case ChatDisplayMode.Expand:
                ShrinkChat();
                break;
        }
    }
    
    private void ExpandChat()
    {
        StartCoroutine(FadeIn(0.5f, chatScroll));
        StartCoroutine(FadeOut(0.5f, chatDisplay));
        _chatDisplayMode = ChatDisplayMode.Expand;
        Debug.Log("Chat expanded.");
    }

    private void ShrinkChat()
    {
        StartCoroutine(FadeOut(0.5f, chatScroll));
        StartCoroutine(FadeIn(0.5f, chatDisplay));
        _chatDisplayMode = ChatDisplayMode.Shrink;
        Debug.Log("Chat shrunk.");
    }
    
    public IEnumerator FadeIn(float seconds, CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
        yield return canvasGroup.DOFade(1f, seconds).WaitForCompletion();
    }

    public IEnumerator FadeOut(float seconds, CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        yield return canvasGroup.DOFade(0f, seconds).WaitForCompletion();
    }
}