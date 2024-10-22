using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Map
{
    public class MapManager: MonoBehaviour
    {
        public static MapManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
            DontDestroyOnLoad(gameObject);
        }

        public async UniTask<List<MapInfo>> LoadMapList(User user)
        {
            return await LoadDefaultsMapList();
        }

        public async UniTask<MapConcrete> LoadMap(MapInfo mapInfo)
        {
            return await MapConverter.ConvertMapInfoToMapConcrete(mapInfo);
        }

        private async UniTask<List<MapInfo>> LoadDefaultsMapList()
        {
            List<MapInfo> mapInfos = new List<MapInfo>(4);
            mapInfos.Add(JsonConvert.DeserializeObject<MapInfo>(await LoadMapFileFromResources("DummyMap1")));

            return mapInfos;
        }

        private async UniTask<string> LoadMapFileFromResources(string mapName)
        {
            TextAsset textAsset = await Resources.LoadAsync<TextAsset>("Maps/" + mapName) as TextAsset;
            return textAsset.text;
        }
    }
}