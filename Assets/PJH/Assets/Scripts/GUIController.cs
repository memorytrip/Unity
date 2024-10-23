using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private bool isActive = false;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
    }

    void StartFadeIn(float seconds, CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeIn(seconds, canvasGroup));
    }
    
    void StartFadeOut(float seconds, CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeOut(seconds, canvasGroup));
    }
    
    public IEnumerator FadeIn(float seconds , CanvasGroup canvasGroup)
    {
        if (isActive) yield break;
        isActive = true;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
        yield return canvasGroup.DOFade(1f, seconds).WaitForCompletion();
        
    }

    public IEnumerator FadeOut(float seconds, CanvasGroup canvasGroup)
    {
        if (!isActive) yield break;
        isActive = false;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        yield return canvasGroup.DOFade(0f, seconds).WaitForCompletion();
    }
    
    
}
