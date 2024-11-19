using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

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
            mapInfos.AddRange(await LoadDefaultsMapListFromServer());
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
                mapInfo.type = MapInfo.MapType.Default;
                mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }
        
        private async UniTask<List<MapInfo>> LoadDefaultsMapListFromServer()
        {
            string rawData = await DataManager.Get("/api/map");
            List<MapInfo> mapInfos = MapConverter.ConvertJsonToMapList(rawData);
            foreach (var mapInfo in mapInfos)
            {
                mapInfo.type = MapInfo.MapType.Default;
            }
            return mapInfos;
        }
    #endregion
    #region LoadCustomMapList
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
                mapInfo.type = MapInfo.MapType.Custom;
                mapInfos.Add(mapInfo);
            }

            return mapInfos;
        }
        
        private async UniTask<List<MapInfo>> LoadCustomMapListFromServer()
        {
            string data = await DataManager.Get("/api/custom-map");
            List<MapInfo> mapInfos = MapConverter.ConvertJsonToMapList(data);
            foreach (var mapInfo in mapInfos)
            {
                mapInfo.type = MapInfo.MapType.Custom;
            }
            return mapInfos;
        }
    #endregion

        public async UniTask<MapInfo> LoadMapInfoFromServer(long mapId)
        {
            try
            {
                return await LoadDefaultMapInfoFromServer(mapId);
            }
            catch (UnityWebRequestException e)
            {
                if (e.ResponseCode != 400) 
                    throw e;
            }
            return await LoadCustomMapInfoFromServer(mapId);
        }
        
    #region LoadDefaultMapInfo
        public async UniTask<MapInfo> LoadDefaultMapInfoFromServer(long mapId)
        {
            string rawData = await DataManager.Get($"/api/map/{mapId}");
            MapInfo mapInfo = MapConverter.ConvertJsonToMapInfo(rawData);
            return mapInfo;
        }
    #endregion
    #region LoadCustomMapInfo
        public async UniTask<MapInfo> LoadCustomMapInfoFromServer(long mapId)
        {
            string rawData = await DataManager.Get($"/api/custom-map/{mapId}");
            MapInfo mapInfo = MapConverter.ConvertJsonToMapInfo(rawData);
            return mapInfo;
        }
    #endregion
        private async UniTask<string> LoadMapFileFromResources(string mapName)
        {
            TextAsset textAsset = await Resources.LoadAsync<TextAsset>("Maps/" + mapName) as TextAsset;
            return textAsset.text;
        }
    #region load map thumbnail
        public async UniTask<Sprite> LoadMapThumbnail(MapInfo mapInfo)
        {
            if (Uri.TryCreate(mapInfo.thumbnailUrl, UriKind.Absolute, out Uri uri))
            {
                return await LoadMapThumbnailFromServer(mapInfo.thumbnailUrl);
            }
            else if (File.Exists(mapInfo.thumbnailUrl))
            {
                return await LoadMapThumbnailFromLocal(mapInfo.thumbnailUrl);
            }
            else
            {
                return null;
            }
        }
    
        private async UniTask<Sprite> LoadMapThumbnailFromServer(string uri)
        {
            UnityWebRequest request = new UnityWebRequest(uri, "GET");
            request.downloadHandler = new DownloadHandlerTexture();
            
            Sprite sprite;
            
            try
            {
                await request.SendWebRequest();
            }
            catch (UnityWebRequestException e)
            {
                return null;
            }
            finally
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
                else
                {
                    sprite = null;
                }
            }
            return sprite;
        }
    
        private async UniTask<Sprite> LoadMapThumbnailFromLocal(string path)
        {
            if (File.Exists(path))
            {
                byte[] rawdata = await DataManager.Read(path);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(rawdata))
                {
                    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            }
            return null;
        }
        #endregion

    }
}