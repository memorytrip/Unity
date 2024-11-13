using System.IO;
using Common;
using Cysharp.Threading.Tasks;
using Map;
using Map.Editor;
using Myroom;
using Newtonsoft.Json;
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
            exitButton.onClick.AddListener(()=>Exit().Forget());
        }

        private async UniTaskVoid DownloadMap()
        {
            Debug.Log(originMapId);
            MapInfo mapInfo = await MapManager.Instance.LoadMapInfoFromServer(originMapId);
            mapConcrete = await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
            mapEditorGUI.target.mapConcrete = mapConcrete;
        }

        private async UniTaskVoid Exit()
        {
            await WriteMapToFile();
            string response = await UploadMap();
            CreateMapResponse res = JsonConvert.DeserializeObject<CreateMapResponse>(response);
            LoadMyroom.mapId = res.customMapId;
            LoadMyroom.mapType = MapInfo.MapType.Custom;
            User user = Common.Network.SessionManager.Instance.currentSession.user;
            SceneManager.Instance.MoveRoom($"player_{user.nickName}").Forget();
        }

        private async UniTask<string> UploadMap()
        {
            MapInfo mapInfo = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete, capturer);
            string data = MapConverter.ConvertMapInfoToJson(mapInfo);
            Debug.Log(data);
            return await DataManager.Post($"/api/custom-map/create/{originMapId}", data);
        }

        private async UniTask WriteMapToFile()
        {
            string path = Application.persistentDataPath + "/Maps/";
            string filename = "asdf.json";
            
            MapInfo mapInfo = MapConverter.ConvertMapConcreteToMapInfo(mapConcrete, capturer);
            
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            await File.WriteAllTextAsync(path + filename, MapConverter.ConvertMapInfoToJson(mapInfo));
            Debug.Log($"File save: {path + filename}");
        }

        class CreateMapResponse
        {
            public long customMapId;
            public string response;
        }
    }
}