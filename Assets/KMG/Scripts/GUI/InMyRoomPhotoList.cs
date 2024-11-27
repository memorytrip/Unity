using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Myroom;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GUI
{
    public class InMyRoomPhotoList: MonoBehaviour
    {
        [SerializeField] private GameObject photoItemPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Sprite emptySprite;
        [SerializeField] private CanvasGroup imagePanel;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private Button closeImagePanel;

        [Header("베타 시연용")]
        [SerializeField] private PhotoListItem videoItem;
        
        private void Awake()
        {
            closeImagePanel.onClick.AddListener(CloseImagePanel);
        }

        private void Start()
        {
            closeImagePanel.onClick.AddListener(CustomFmodBgmManager.Instance.ResumePlayback);
        }

        private void CloseImagePanel() => Utility.DisablePanel(imagePanel);
        
        public async UniTask RefreshListProcess()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            string owner = LoadMyroom.mapOwnerName;
            PhotoData[] photos = await GetPhotoList(owner);

            List<UniTask> tasks = new List<UniTask>();
            foreach (var photo in photos)
            {
                tasks.Add(InitItem(photo));
            }
            await UniTask.WhenAll(tasks);
            
            // 베타 시연용
            var obj = Instantiate(videoItem, content).GetComponent<PhotoListItem>();
            var videoPlayer = obj.GetComponentInChildren<VideoPlayer>();
            if (videoPlayer != null)
            {
                var audioSource = videoPlayer.gameObject.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = videoPlayer.gameObject.AddComponent<AudioSource>();
                }
                videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
                videoPlayer.SetTargetAudioSource(0, audioSource);
                audioSource.outputAudioMixerGroup = null;
                audioSource.playOnAwake = false;
                videoPlayer.playOnAwake = false;
                videoPlayer.Stop();
            }
            obj.imagePanel = imagePanel;
            obj.rawImage = rawImage;
            obj.closeButton = closeImagePanel;
        }

        private async UniTask<PhotoData[]> GetPhotoList(string name)
        {
            string data = await DataManager.Get($"/api/photos/all/{name}");
            Debug.Log(data);
            return JsonConvert.DeserializeObject<PhotoData[]>(data);
        }

        private async UniTask<PhotoListItem> InitItem(PhotoData photo)
        {
            var obj = Instantiate(photoItemPrefab, content).GetComponent<PhotoListItem>();
            Sprite thumbnail = await LoadPhoto(photo.photoUrl);
            obj.imagePanel = imagePanel;
            obj.rawImage = rawImage;
            if (thumbnail == null)
                thumbnail = emptySprite;
            obj.SetThumbnail(thumbnail);
            return obj;
        }

        private async UniTask<Sprite> LoadPhoto(string url)
        {
            return await DataManager.GetSprite(url);
        }

        class PhotoData
        {
            public long photoId;
            public string photoUrl;
            // public long letterId;
            // public string uploaderEmail;
            // public string finderEmail;
            // public bool isFound;
            // public string roomCode;
        }
    }
}