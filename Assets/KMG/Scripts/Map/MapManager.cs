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

#region Load MapList, MapInfo
        public async UniTask<List<MapInfo>> LoadMapList(User user)
        {
            List<MapInfo> mapInfos = new List<MapInfo>(4);
            mapInfos.AddRange(await LoadDefaultsMapList());
            mapInfos.AddRange(await LoadCustomMapListFromLocal());
            return mapInfos;
        }
        
        private async UniTask<List<MapInfo>> LoadDefaultsMapList()
        {
            List<MapInfo> mapInfos = new List<MapInfo>(4);
            mapInfos.Add(JsonConvert.DeserializeObject<MapInfo>(await LoadMapFileFromResources("DummyMap01")));
            mapInfos.Add(JsonConvert.DeserializeObject<MapInfo>(await LoadMapFileFromResources("DummyMap02")));

            return mapInfos;
        }

        private async UniTask<List<MapInfo>> LoadCustomMapListFromServer()
        {
            List<MapInfo> mapInfos = new List<MapInfo>();
            string data = await DataManager.Get("/api/map/list");
            
            return mapInfos;
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
                MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(await reader.ReadToEndAsync());
                mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }

        private async UniTask<string> LoadMapFileFromResources(string mapName)
        {
            TextAsset textAsset = await Resources.LoadAsync<TextAsset>("Maps/" + mapName) as TextAsset;
            return textAsset.text;
        }
#endregion
        
#region Load MapConcrete        
        public async UniTask<MapConcrete> LoadMap(MapInfo mapInfo)
        {
            return await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
        }
#endregion

        class MapList
        {
            public List<MapInfo> mapInfos;
        }
    }
}