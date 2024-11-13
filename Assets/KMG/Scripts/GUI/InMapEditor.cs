using Common;
using Cysharp.Threading.Tasks;
using Map;
using Map.Editor;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class InMapEditor: MonoBehaviour
    {
        public static long originMapId;
        
        [SerializeField] private MapEditorGUI mapEditorGUI;
        [SerializeField] private Button exitButton;
        [SerializeField] private ThumbnailCapture capturer;
        private MapConcrete mapConcrete;

        private void Start()
        {
            DownloadMap().Forget();
            exitButton.onClick.AddListener(Exit);
        }

        private async UniTaskVoid DownloadMap()
        {
            Debug.Log(originMapId);
            MapInfo mapInfo = await MapManager.Instance.LoadMapInfoFromServer(originMapId);
            mapConcrete = await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
            mapEditorGUI.target.mapConcrete = mapConcrete;
        }

        private void Exit()
        {
            UploadMap().Forget();
            User user = Common.Network.SessionManager.Instance.currentSession.user;
            SceneManager.Instance.MoveRoom($"player_{user.nickName}").Forget();
        }

        private async UniTaskVoid UploadMap()
        {
            MapInfo mapInfo = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete, capturer);
            string data = MapConverter.ConvertMapInfoToJson(mapInfo);
            Debug.Log(data);
            string response = await DataManager.Post($"/api/custom-map/create/{originMapId}", data);
            Debug.Log(response);
        }
    }
}