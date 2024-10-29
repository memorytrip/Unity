using System.Collections;
using DG.Tweening;
using GUI;
using UnityEngine;

public class MenuFader : MonoBehaviour
{
    private bool isActive;
    
    public void StartFadeIn(float seconds , CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeIn(0.5f, canvasGroup));
    }
        
    public void StartFadeOut(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeOut(0.5f , canvasGroup));
    }
    
    private IEnumerator FadeIn(float seconds, CanvasGroup canvasGroup)
    {
        if (isActive) yield break;
        isActive = true;
        yield return canvasGroup.DOFade(1f, seconds).WaitForCompletion();
    }
        
    private IEnumerator FadeOut(float seconds, CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        if (!isActive) yield break;
        isActive = false;
        yield return canvasGroup.DOFade(0f, seconds).WaitForCompletion();
    }
}
