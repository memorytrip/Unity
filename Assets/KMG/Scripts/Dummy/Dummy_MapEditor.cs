using System.IO;
using Common;
using Cysharp.Threading.Tasks;
using Map;
using Map.Editor;
using Newtonsoft.Json;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class Dummy_MapEditor: MonoBehaviour
    {
        [SerializeField] private MapEditorGUI mapEditorGUI;
        [SerializeField] private Button exitButton;
        [SerializeField] private ThumbnailCapture capturer;
        [SerializeField] private CinemachineCamera cam;
        private MapConcrete mapConcrete;

        private static string path;
        private static string filename;

        private async UniTaskVoid Start()
        {
            path = Application.persistentDataPath + "/Maps/";
            filename = "asdf.json";
            
            mapConcrete = await ConvertFileToMapConcrete();
            mapEditorGUI.target.mapConcrete = mapConcrete;
            exitButton.onClick.AddListener(Exit);
        }

        private async UniTask<MapConcrete> ConvertFileToMapConcrete()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            MapInfo mapInfo;
            if (File.Exists(path + filename))
            {
                string rawData = await File.ReadAllTextAsync(path + filename);
                // mapInfo = JsonConvert.DeserializeObject<MapInfo>(rawData);
                mapInfo = MapConverter.ConvertJsonToMapInfo(rawData);
            }
            else
            {
                mapInfo = MapInfo.GetDefaultMap();
            }
            return await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
        }

        private async UniTaskVoid ConvertMapConcreteToFile(MapConcrete mapConcrete)
        {
            MapInfo mapInfo = await MapConverter.ConvertMapConcreteToMapInfo(mapConcrete, capturer);
            
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            await File.WriteAllTextAsync(path + filename, MapConverter.ConvertMapInfoToJson(mapInfo));
            Debug.Log($"File save: {path + filename}");
        }

        private async UniTaskVoid SendMapToServer(MapConcrete mapConcrete)
        {
            MapInfo mapInfo = await MapConverter.ConvertMapConcreteToMapInfo(mapConcrete, capturer);
            string data = MapConverter.ConvertMapInfoToJson(mapInfo);
            
            Debug.Log(data);
            // string response = await DataManager.Post("/api/map/create", data);
            string response = await DataManager.Post("/api/custom-map/create/1", data);
            Debug.Log(response);
        }

        private void Exit()
        {
            ConvertMapConcreteToFile(mapConcrete).Forget();
            SendMapToServer(mapConcrete).Forget();
            User user = Common.Network.SessionManager.Instance.currentSession.user;
            SceneManager.Instance.MoveRoom($"player_{user.nickName}").Forget();
        }
    }
}