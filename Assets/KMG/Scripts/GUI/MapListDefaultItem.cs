using System;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using GUI;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class MapListDefaultItem : MonoBehaviour
    {
        [HideInInspector] public string mapId;
        [SerializeField] private Image thumbnail;
        [SerializeField] private Button editButton;
        [SerializeField] private Button setHomeButton;

        private void Awake()
        {
            editButton.onClick.AddListener(Edit);
            setHomeButton.onClick.AddListener(SetHome);
        }

        public void SetThumbnail(Sprite sprite)
        {
            thumbnail.sprite = sprite;
        }

        private void Edit()
        {
            EnterEditMode().Forget();
        }

        private async UniTaskVoid EnterEditMode()
        {
            InMapEditor.originMapId = long.Parse(mapId);
            await RunnerManager.Instance.Disconnect();
            await SceneManager.Instance.MoveSceneProcess("MapEdit");
        }

        private void SetHome()
        {
            // TODO: 마이룸 설정 과정
            string playerId = SessionManager.Instance.currentUser.nickName;
            SceneManager.Instance.MoveRoom($"player_{playerId}").Forget();
        }
    }
}