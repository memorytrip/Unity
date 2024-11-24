using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Myroom;
using Newtonsoft.Json;
using UnityEngine;

namespace GUI
{
    public class InMyRoomPhotoList: MonoBehaviour
    {
        [SerializeField] private GameObject photoItemPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Sprite emptySprite;

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