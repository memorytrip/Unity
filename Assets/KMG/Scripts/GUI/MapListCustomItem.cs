using System;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using GUI;
using Map;
using Myroom;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GUI
{
    public class MapListCustomItem : MonoBehaviour
    {
        [HideInInspector] public long mapId;
        [HideInInspector] public InMyroomMapList mapList;
        [SerializeField] private Image thumbnail;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button setHomeButton;

        private void Awake()
        {
            deleteButton.onClick.AddListener(DeletePopup);
            setHomeButton.onClick.AddListener(SetHome);
        }

        public void SetThumbnail(Sprite sprite)
        {
            thumbnail.sprite = sprite;
        }

        private void DeletePopup()
        {
            mapList.confirmDeletePanel.SetActive(true);
            mapList.confirmDeleteButton.onClick.RemoveAllListeners();
            mapList.confirmDeleteButton.onClick.AddListener(Delete);
        }

        private void Delete()
        {
            mapList.confirmDeletePanel.SetActive(false);
            DeleteProcess().Forget();
        }

        private async UniTaskVoid DeleteProcess()
        {
            await DataManager.Delete($"/api/custom-map/{mapId}");
            mapList.RefreshListProcess().Forget();
        }
        
        private void SetHome() => SetHomeProcess().Forget();

        private async UniTaskVoid SetHomeProcess()
        {
            await DataManager.Post($"/api/main-map/custom-map/{mapId}");
            string playerId = SessionManager.Instance.currentUser.nickName;
            SceneManager.Instance.MoveRoom($"player_{playerId}").Forget();
        }
    }

}