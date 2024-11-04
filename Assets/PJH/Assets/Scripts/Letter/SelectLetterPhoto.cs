using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SelectLetterPhoto : MonoBehaviour
{
    private bool isActive;

    void Awake()
    {
        isActive = false;
    }
    
    public void ShowLetter(CanvasGroup c)
    {
        StartCoroutine(FadeIn(0.5f, c));
    }

    public void HideLetter(CanvasGroup c)
    {
       StartCoroutine(FadeOut(0.5f ,c));
    }
    
    private IEnumerator FadeIn(float seconds , CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
        yield return canvasGroup.DOFade(1f, seconds).WaitForCompletion();
        
    }

    private IEnumerator FadeOut(float seconds, CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        yield return canvasGroup.DOFade(0f, seconds).WaitForCompletion();
    }
}
