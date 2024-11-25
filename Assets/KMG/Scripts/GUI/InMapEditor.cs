using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Network;
using Cysharp.Threading.Tasks;
using Map;
using Map.Editor;
using Myroom;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
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
            SetHome(res.customMapId);
            User user = Common.Network.SessionManager.Instance.currentSession.user;
            SceneManager.Instance.MoveRoom($"player_{user.nickName}").Forget();
        }

        private async UniTask<string> UploadMap()
        {
            MapInfo mapInfo = await MapConverter.ConvertMapConcreteToMapInfo(mapConcrete, capturer);
            string data = MapConverter.ConvertMapInfoToJson(mapInfo);
            Debug.Log(data);
            List<IMultipartFormSection> formdata = new List<IMultipartFormSection>();
            formdata.Add(new MultipartFormDataSection("data", data, "application/json"));
            formdata.Add(new MultipartFormFileSection("thumbnail", mapInfo.thumbnail, $"thumbnail{Guid.NewGuid().ToString()}", ""));
            return await DataManager.Post($"/api/custom-map/create/{originMapId}", formdata);
        }
        
        private void SetHome(long mapId) => SetHomeProcess(mapId).Forget();

        private async UniTaskVoid SetHomeProcess(long mapId)
        {
            await DataManager.Post($"/api/main-map/custom-map/{mapId}");
            string playerId = SessionManager.Instance.currentUser.nickName;
            SceneManager.Instance.MoveRoom($"player_{playerId}").Forget();
        }

        private async UniTask WriteMapToFile()
        {
            string path = Application.persistentDataPath + "/Maps/";
            string filename = "asdf.json";
            string thumbnailName = "asdf.png";
            
            MapInfo mapInfo = await MapConverter.ConvertMapConcreteToMapInfo(mapConcrete, capturer);
            
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            await File.WriteAllTextAsync(path + filename, MapConverter.ConvertMapInfoToJson(mapInfo));
            await File.WriteAllBytesAsync(path + thumbnailName, mapInfo.thumbnail);
            Debug.Log($"File save: {path + filename}");
        }

        class CreateMapResponse
        {
            public long customMapId;
            public string response;
        }
    }
}