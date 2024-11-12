using System;
using System.Collections;
using DG.Tweening;
using GUI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class ChatDisplay : MonoBehaviour
{
    public enum ChatDisplayMode
    {
        Shrink,
        Expand,
    }
    
    public bool isTyping;
    
    public event Action<bool> StartTyping;

    public void OnStartedTyping(bool typingStatus)
    {
        StartTyping?.Invoke(typingStatus);
    }
    
    public event Action<bool> StopTyping;

    public void OnStoppedTyping(bool typingStatus)
    {
        StopTyping?.Invoke(typingStatus);
    }
    
    public CanvasGroup _chatDisplay;
    public CanvasGroup chatScroll;
    private TMP_InputField _inputField;
    [SerializeField] private Button expandChatButton;
    private ChatDisplayMode _chatDisplayMode;
    private const float MinHeight = 100f;
    private const float MaxHeight = 400f;
    
    private void Awake()
    {
        //_chatDisplayArea = GetComponent<RectTransform>();
        //_chatDisplayArea.sizeDelta = new Vector2(_chatDisplayArea.sizeDelta.x, MinHeight);
        _chatDisplayMode = ChatDisplayMode.Shrink;
        
        expandChatButton.onClick.AddListener(ToggleChat);
        
        _inputField = GetComponentInChildren<TMP_InputField>();
        _inputField.restoreOriginalTextOnEscape = true;
        
        _inputField.onSelect.AddListener(StartChat);
        _inputField.onSubmit.AddListener(SubmitChat);
        _inputField.onEndEdit.AddListener(StopChat);
        _inputField.onDeselect.AddListener(StopChat);
    }

    private void StartChat(string text)
    {
        if (isTyping)
        {
            return;
        }
        _inputField.ActivateInputField();
        Debug.Log($"StartChat: {text}");
        isTyping = true;
        OnStartedTyping(isTyping);
        Debug.Log("얼음땡: 이게 안되냐");
        Debug.Log($"얼음땡 IsTyping: {isTyping}");
    }

    private void StopChat(string text)
    {
        if (isTyping)
        {
            Debug.Log($"StopChat: {text}");
            _inputField.DeactivateInputField(true);
            isTyping = false;
            OnStoppedTyping(isTyping);
            Debug.Log("얼음땡: 이게 안되냐");
            Debug.Log($"얼음땡 IsTyping: {isTyping}");
        }
    }

    private void SubmitChat(string text)
    {
        if (isTyping)
        {
            Debug.Log($"SubmitChat: {text}");
            _inputField.text = "";
            //OnStoppedTyping(isTyping);
            _inputField.DeactivateInputField(true);
            isTyping = false;
            Debug.Log($"얼음땡 IsTyping: {isTyping}");
        }
    }

    private void ToggleChat()
    {
        switch (_chatDisplayMode)
        {
            case ChatDisplayMode.Shrink:
                StartCoroutine(FadeIn(0.5f, chatScroll));
                StartCoroutine(FadeOut(0.5f, _chatDisplay));
                _inputField.enabled = true;
                _inputField.interactable = true;
                _chatDisplayMode = ChatDisplayMode.Expand;
                Debug.Log("Expand chat");
                break;
            case ChatDisplayMode.Expand:
                StartCoroutine(FadeOut(0.5f, chatScroll));
                StartCoroutine(FadeIn(0.5f, _chatDisplay));
                _inputField.enabled = false;
                _inputField.interactable = false;
                _chatDisplayMode = ChatDisplayMode.Shrink;
                Debug.Log("Shrink chat");
                break;
        }
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