using System.Collections.Generic;
using System.IO;
using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Map
{
    public class MapManager
    {
        private static MapManager _instance;
        public static MapManager Instance
        {
            get
            {
                if (_instance == null) _instance = new MapManager();
                return _instance;
            }
        }
        private MapManager()
        {
            if (_instance != null) 
                return;
        }
        
        public async UniTask<List<MapInfo>> LoadMapList(User user)
        {
            List<MapInfo> mapInfos = new List<MapInfo>(4);
            mapInfos.AddRange(await LoadDefaultsMapListFromLocal());
            mapInfos.AddRange(await LoadCustomMapListFromServer());
            return mapInfos;
        }
#region LoadDefaultsMapList
        private async UniTask<List<MapInfo>> LoadDefaultsMapListFromLocal()
        {
            string[] defaultsMapName = new[] { "map1", "map2", "map3", "map4" };
            List<MapInfo> mapInfos = new List<MapInfo>(4);
            foreach (var mapName in defaultsMapName)
            {
                string rawdata = await LoadMapFileFromResources(mapName);
                MapInfo mapInfo = MapConverter.ConvertJsonToMapInfo(rawdata);
                mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }
        
        private async UniTask<List<MapInfo>> LoadDefaultsMapListFromServer()
        {
            string rawData = await DataManager.Get("/api/map");
            List<MapInfo> mapInfos = MapConverter.ConvertJsonToMapList(rawData);
            return mapInfos;
        }
#endregion
#region LoadCustomMapList
        private async UniTask<List<MapInfo>> LoadCustomMapListFromServer()
        {
            List<MapInfo> mapIndices = new List<MapInfo>();
            string data = await DataManager.Get("/api/custom-map");
            mapIndices = MapConverter.ConvertJsonToMapList(data);
            
            return mapIndices;
        }

        private async UniTask<List<MapInfo>> LoadCustomMapListFromLocal()
        {
            List<MapInfo> mapInfos = new List<MapInfo>();
            
            DirectoryInfo directoryInfo = new DirectoryInfo($"{Application.persistentDataPath}/Maps/");
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            foreach (var mapJson in directoryInfo.GetFiles("*.json"))
            {
                StreamReader reader = mapJson.OpenText();
                MapInfo mapInfo = MapConverter.ConvertJsonToMapInfo(await reader.ReadToEndAsync());
                mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }
#endregion
        private async UniTask<string> LoadMapFileFromResources(string mapName)
        {
            TextAsset textAsset = await Resources.LoadAsync<TextAsset>("Maps/" + mapName) as TextAsset;
            return textAsset.text;
        }
    }
}