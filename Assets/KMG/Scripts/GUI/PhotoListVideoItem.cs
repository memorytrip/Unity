using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GUI
{
    public class PhotoListVideoItem: PhotoListItem
    {
        [SerializeField] private VideoPlayer videoPlayer;

        protected override void ShowImage()
        {
            CustomFmodBgmManager.Instance.StopPlayback();
            Utility.EnablePanel(imagePanel);
            rawImage.texture = videoPlayer.targetTexture;
            StartCoroutine(PlayVideoProcess());
            closeButton.onClick.AddListener(CloseImage);
        }

        private IEnumerator PlayVideoProcess()
        {
            videoPlayer.Prepare();
            yield return new WaitUntil( () => videoPlayer.isPrepared );
            videoPlayer.Play();
        }

        private void CloseImage()
        {
            videoPlayer.Stop();
            closeButton.onClick.RemoveListener(CloseImage);
        }
    }
}