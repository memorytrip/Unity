using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class VideoProgressBar : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private float maxTime = 30f;
    [SerializeField] private float currentTime = 0f;
    public CanvasGroup doneCanvasGroup;
    public bool isLoading;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        UIManager.HideUI(canvasGroup);
        UIManager.HideUI(doneCanvasGroup);
        isLoading = false;
    }

    public void Update()
    {
        VideoLoadProgress();
    }

    private void VideoLoadProgress()
    {
        if (!isLoading)
        {
            return;
        }
        
        UIManager.ShowUI(canvasGroup);
        if (currentTime <= maxTime)
        {
            currentTime += Time.unscaledDeltaTime;
        }
        else
        {
            UIManager.HideUI(canvasGroup);
            UIManager.ShowUI(doneCanvasGroup);
        }
    }
}
