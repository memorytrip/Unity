using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SelectLetterPhoto : MonoBehaviour
{
    //public GameObject letterPhoto;
    public float turnSpeed = 7200f;
    private bool isActive;

    void Awake()
    {
        isActive = false;
    }
    
    public void ShowLetter(CanvasGroup c)
    {
        StartCoroutine(FadeIn(c));
    }

    public void HideLetter(CanvasGroup c)
    {
        StartCoroutine(FadeOut(c));
    }
    
    public IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        if (isActive) yield break;
        isActive = true;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;
        yield return canvasGroup.DOFade(1f, 0.5f).WaitForCompletion();
    }

    public IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        if (!isActive) yield break;
        isActive = false;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        yield return canvasGroup.DOFade(0f, 0.5f).WaitForCompletion();
    }
}
