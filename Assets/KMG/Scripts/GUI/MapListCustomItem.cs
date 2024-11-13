using System;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using GUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GUI
{
    public class MapListCustomItem : MonoBehaviour
    {
        [HideInInspector] public string mapId;
        [HideInInspector] public InMyroomMapList mapList;
        [SerializeField] private Image thumbnail;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button setHomeButton;

        private void Awake()
        {
            deleteButton.onClick.AddListener(Delete);
            setHomeButton.onClick.AddListener(SetHome);
        }

        public void SetThumbnail(Sprite sprite)
        {
            thumbnail.sprite = sprite;
        }

        private void Delete()
        {
            DeleteProcess().Forget();
        }

        private async UniTaskVoid DeleteProcess()
        {
            await DataManager.Delete($"/api/custom-map/{mapId}");
            mapList.RefreshListProcess().Forget();
        }

        private void SetHome()
        {
            string playerId = SessionManager.Instance.currentUser.nickName;
            SceneManager.Instance.MoveRoom($"player_{playerId}").Forget();
        }
    }

}