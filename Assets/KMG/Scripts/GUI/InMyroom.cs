using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Map;
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
        
        [Header("Buy Map")]
        [SerializeField] private CanvasGroup buyMapPanel;
        [SerializeField] private Button buyMapButton;
        [SerializeField] private Button closeBuyMapPanel;

        private void Start()
        {
            returnToSquareButton.onClick.AddListener(ReturnToSquare);
            closeMapListButton.onClick.AddListener(CloseSetting);
            
            openVisitRoom.onClick.AddListener(OpenVisitRoom);
            closeVisitRoom.onClick.AddListener(CloseVisitRoom);
            enterVisitRoom.onClick.AddListener(VisitRoom);
            
            openAlbumPanel.onClick.AddListener(OpenAlbumPanel);
            closeAlbumPanel.onClick.AddListener(CloseAlbumPanel);

            enterEditModeButton.interactable = false;
            ActiveSetting().Forget();
            
            buyMapButton.onClick.AddListener(BuyMap);
            closeBuyMapPanel.onClick.AddListener(CloseBuyMapPanel);
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

        private void OpenBuyMapPanel()
        {
            Utility.EnablePanel(buyMapPanel);
        }

        private void CloseBuyMapPanel()
        {
            Utility.DisablePanel(buyMapPanel);
        }

        private void BuyMap()
        {
            BuyMapProcess().Forget();
        }

        private async UniTaskVoid BuyMapProcess()
        {
            MapInfo mapInfo = await MapManager.Instance.LoadMyroomMapInfoFromServer(LoadMyroom.mapOwnerName);
            if (mapInfo.type == MapInfo.MapType.Default)
            {
                PopupManager.Instance.ShowMessage("이미 보유한 기본 맵입니다.");
                return;
            }

            try
            {
                await DataManager.Post($"/api/main-map/sell/{mapInfo.mainMapId}");
                PopupManager.Instance.ShowMessage("구매되었습니다.");
            }
            catch (UnityWebRequestException e)
            {
                PopupManager.Instance.ShowMessage(e);
            }
            
        }

        private async UniTaskVoid ActiveSetting()
        {
            // await UniTask.WaitWhile(() => string.IsNullOrEmpty(SceneManager.Instance.curScene) );
            await UniTask.WaitWhile(() => string.IsNullOrEmpty(RunnerManager.Instance.Runner?.SessionInfo.Name));
            string sessionName = RunnerManager.Instance.Runner?.SessionInfo.Name;
            string myName = SessionManager.Instance.currentUser.nickName;
            Debug.Log($"InMyRoom.ActiveSetting: {RunnerManager.Instance.Runner?.SessionInfo.Name}");
            enterEditModeButton.onClick.RemoveAllListeners();
            if (sessionName == $"player_{myName}")
            {
                enterEditModeButton.interactable = true;
                enterEditModeButton.onClick.AddListener(EnterSetting);
            }
            else
            {
                enterEditModeButton.interactable = true;
                enterEditModeButton.onClick.AddListener(OpenBuyMapPanel);
            }
        }
    }
}