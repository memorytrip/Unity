using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Map;
using Myroom;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class MapListDefaultItem : MonoBehaviour
    {
        [HideInInspector] public long mapId;
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
            InMapEditor.originMapId = mapId;
            await RunnerManager.Instance.Disconnect();
            await SceneManager.Instance.MoveSceneProcess("MapEdit");
        }

        private void SetHome() => SetHomeProcess().Forget();
        private async UniTaskVoid SetHomeProcess()
        {
            await DataManager.Post($"/api/main-map/map/{mapId}");
            string playerId = SessionManager.Instance.currentUser.nickName;
            SceneManager.Instance.MoveRoom($"player_{playerId}").Forget();
        }
    }
}