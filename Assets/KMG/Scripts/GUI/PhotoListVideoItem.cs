using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GUI
{
    public class PhotoListVideoItem: PhotoListItem
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [HideInInspector] public string videoUrl;

        protected override void ShowImage()
        {
            CustomFmodBgmManager.Instance.StopPlayback();
            Utility.EnablePanel(imagePanel);
            rawImage.texture = videoPlayer.targetTexture;
            StartCoroutine(PlayVideoProcess());
            closeButton.onClick.AddListener(CloseImage);
            
            photoList.downloadButton.onClick.RemoveAllListeners();
            photoList.downloadButton.onClick.AddListener(Download);
        }

        private IEnumerator PlayVideoProcess()
        {
            videoPlayer.url = videoUrl;
            videoPlayer.Prepare();
            yield return new WaitUntil( () => videoPlayer.isPrepared );
            videoPlayer.Play();
        }

        private void CloseImage()
        {
            videoPlayer.Stop();
            closeButton.onClick.RemoveListener(CloseImage);
        }

        protected override void Download()
        {
            string path = Application.persistentDataPath + "/media";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = Guid.NewGuid().ToString() + ".mp4";
            
            
            UnityWebRequest request = UnityWebRequest.Get(videoUrl);
            request.downloadHandler = new DownloadHandlerFile($"{path}/{filename}");
            request.SendWebRequest();
            
            Debug.Log($"Download Picture: {path}/{filename}");
        }
    }
}