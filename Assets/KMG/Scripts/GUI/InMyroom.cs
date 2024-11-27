using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Myroom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class InMyroom: MonoBehaviour
    {
        [SerializeField] private Button returnToSquareButton;
        [SerializeField] private Button enterEditModeButton;
        
        [Header("Map List")]
        [SerializeField] private InMyroomMapList maplist;
        [SerializeField] private CanvasGroup mapListPanel;
        [SerializeField] private Button closeMapListButton;

        [Header("Visit Other Room")] 
        [SerializeField] private CanvasGroup visitRoomPanel;
        [SerializeField] private Button openVisitRoom;
        [SerializeField] private Button closeVisitRoom;
        [SerializeField] private TMP_InputField inputVisitRoom;
        [SerializeField] private Button enterVisitRoom;

        [Header("Album")] 
        [SerializeField] private InMyRoomPhotoList album;
        [SerializeField] private CanvasGroup albumPanel;
        [SerializeField] private Button openAlbumPanel;
        [SerializeField] private Button closeAlbumPanel;

        private void Start()
        {
            returnToSquareButton.onClick.AddListener(ReturnToSquare);
            enterEditModeButton.onClick.AddListener(EnterSetting);
            closeMapListButton.onClick.AddListener(CloseSetting);
            
            openVisitRoom.onClick.AddListener(OpenVisitRoom);
            closeVisitRoom.onClick.AddListener(CloseVisitRoom);
            enterVisitRoom.onClick.AddListener(VisitRoom);
            
            openAlbumPanel.onClick.AddListener(OpenAlbumPanel);
            closeAlbumPanel.onClick.AddListener(CloseAlbumPanel);

            enterEditModeButton.interactable = false;
            ActiveSetting().Forget();
        }

        private void ReturnToSquare()
        {
            SceneManager.Instance.MoveRoom(SceneName.Square).Forget();
        }

        private void EnterSetting()
        {
            Utility.EnablePanel(mapListPanel);
            maplist.RefreshListProcess().Forget();
        }

        private void CloseSetting()
        {
            Utility.DisablePanel(mapListPanel);
        }

        private void OpenVisitRoom()
        {
            Utility.EnablePanel(visitRoomPanel);
        }

        private void CloseVisitRoom()
        {
            Utility.DisablePanel(visitRoomPanel);
        }

        private void VisitRoom()
        {
            string roomName = $"player_{inputVisitRoom.text}";
            LoadMyroom.mapOwnerName = inputVisitRoom.text;
            SceneManager.Instance.MoveRoom(roomName).Forget();
        }

        private void OpenAlbumPanel()
        {
            Utility.EnablePanel(albumPanel);
            album.RefreshListProcess().Forget();
        }

        private void CloseAlbumPanel()
        {
            Utility.DisablePanel(albumPanel);
        }

        private async UniTaskVoid ActiveSetting()
        {
            // await UniTask.WaitWhile(() => string.IsNullOrEmpty(SceneManager.Instance.curScene) );
            await UniTask.WaitWhile(() => string.IsNullOrEmpty(RunnerManager.Instance.Runner?.SessionInfo.Name));
            string sessionName = RunnerManager.Instance.Runner?.SessionInfo.Name;
            string myName = SessionManager.Instance.currentUser.nickName;
            Debug.Log($"InMyRoom.ActiveSetting: {RunnerManager.Instance.Runner?.SessionInfo.Name}");
            if (sessionName == $"player_{myName}")
            {
                enterEditModeButton.interactable = true;
            }
        }
    }
}