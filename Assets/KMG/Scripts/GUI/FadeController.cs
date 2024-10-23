using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace GUI
{
    public class FadeController: MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        private bool isActive = false;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        public void StartFadeIn(float seconds)
        {
            StartCoroutine(FadeIn(seconds));
        }

        public void StartFadeOut(float seconds)
        {
            StartCoroutine(FadeOut(seconds));
        }

        public IEnumerator FadeIn(float seconds)
        {
            if (isActive) yield break;
            isActive = true;
            yield return canvasGroup.DOFade(1f, seconds).WaitForCompletion();
        }

        public IEnumerator FadeOut(float seconds)
        {
            if (!isActive) yield break;
            isActive = false;
            yield return canvasGroup.DOFade(0f, seconds).WaitForCompletion();
        }
    }
}