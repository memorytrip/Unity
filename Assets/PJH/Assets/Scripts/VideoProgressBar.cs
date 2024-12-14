using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoProgressBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;     
    [SerializeField] private CanvasGroup doneCanvasGroup;
    [SerializeField] private VideoPlayer videoPlayer;
    
    [SerializeField] private bool isLoading = false;   
    [SerializeField] private float currentTime = 0f;    
    private float maxTime = 60f;      
    
    public void StartLoading()
    {
        if (!isLoading) 
        {
            isLoading = true;
            currentTime = 0f; 
            StartCoroutine(StartVideoLoading());
        }
    }
    
    private IEnumerator StartVideoLoading()
    {
        while (isLoading)
        {
            VideoLoadProgress();
            videoPlayer.Play();
            yield return null;
        }
    }
    
    private void VideoLoadProgress()
    {
        if (!ShouldContinueLoading())  
        {
            return;
        }

        UpdateLoadingProgress();
    }
    
    private bool ShouldContinueLoading()
    {
        return isLoading;
    }
    
    private void UpdateLoadingProgress()
    {
        UIManager.ShowUI(canvasGroup);

        if (currentTime <= maxTime)
        {
            currentTime += Time.unscaledDeltaTime;
        }
        else
        {
            FinishLoading();
        }
    }
    
    private void FinishLoading()
    {
        isLoading = false;
        UIManager.HideUI(canvasGroup);
        UIManager.ShowUI(doneCanvasGroup);
    }
}