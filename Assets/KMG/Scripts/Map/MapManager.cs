using System.Collections.Generic;
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
            if (_instance != null) return;
        }

        public async UniTask<List<MapInfo>> LoadMapList(User user)
        {
            return await LoadDefaultsMapList();
        }
        
        private async UniTask<List<MapInfo>> LoadDefaultsMapList()
        {
            List<MapInfo> mapInfos = new List<MapInfo>(4);
            mapInfos.Add(JsonConvert.DeserializeObject<MapInfo>(await LoadMapFileFromResources("DummyMap01")));
            mapInfos.Add(JsonConvert.DeserializeObject<MapInfo>(await LoadMapFileFromResources("DummyMap02")));

            return mapInfos;
        }

        private async UniTask<string> LoadMapFileFromResources(string mapName)
        {
            TextAsset textAsset = await Resources.LoadAsync<TextAsset>("Maps/" + mapName) as TextAsset;
            return textAsset.text;
        }
        
        public async UniTask<MapConcrete> LoadMap(MapInfo mapInfo)
        {
            return await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
        }
    }
}