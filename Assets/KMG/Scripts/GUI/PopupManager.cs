using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        Instance = this;
        closeButton.onClick.AddListener(HideMessage);
    }

    public void ShowMessage(string message)
    {
        UIManager.ShowUI(canvasGroup);
        text.text = message;
    }

    public void ShowMessage(UnityWebRequestException e)
    {
        UIManager.ShowUI(canvasGroup);
        if (e.ResponseHeaders != null && e.ResponseHeaders["Content-Type"] == "application/json")
        {
            ErrorMessage message = JsonConvert.DeserializeObject<ErrorMessage>(e.Text);
            text.text = message.detailMessage;
        }
        else
        {
            text.text = e.Error;
        }
    }

    private void HideMessage()
    {
        UIManager.HideUI(canvasGroup);
    }
    
}

class ErrorMessage
{
    public string errorCode;
    public string message;
    public string detailMessage;
}
