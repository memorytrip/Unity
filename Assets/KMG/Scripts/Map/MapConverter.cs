using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Map.Editor;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Map
{
    /**
     * Json <-> MapInfo <-> MapConcrete 변환
     */
    public class MapConverter
    {
        public static async UniTask<MapConcrete> ConvertMapInfoToMapConcrete(MapInfo info)
        {
            MapData mapData = info.data;
            
            // MapConcrete 초기화
            MapConcrete mapConcrete = new MapConcrete();
            mapConcrete.isLoading = true;
            mapConcrete.info = info;
            await mapConcrete.SetTheme(mapData.themeId);
            
            // MapConcrete에 MapObject 추가
            foreach (var mapObjectData in mapData.mapObjectList)
            {
                Model model = await ModelManager.Instance.Get(mapObjectData);
                mapConcrete.AddMapObject(mapObjectData.position.ToVector3(), mapObjectData.rotation.ToQuaternion(), model);
            }

            mapConcrete.isLoading = false;
            return mapConcrete;
        }
        

        public static async UniTask<MapInfo> ConvertMapConcreteToMapInfo(MapConcrete mapConcrete, ThumbnailCapture capture)
        {
            MapData mapData = new MapData();
            mapData.themeId = mapConcrete.theme.id;
            mapData.mapObjectList = new List<MapData.MapObjectData>(mapConcrete.mapObjects.Count);
            foreach (var mapObject in mapConcrete.mapObjects)
            {
                MapData.MapObjectData objData = new MapData.MapObjectData();
                objData.modelId = mapObject.GetModel().id;
                objData.position = new SerializedVector3(mapObject.transform.position);
                objData.rotation = new SerializedQuaternion(mapObject.transform.rotation); 
                mapData.mapObjectList.Add(objData);
            }

            //TODO: mapInfo.id, mapInfo.thmbnail 채우기
            MapInfo mapInfo = new MapInfo();
            mapInfo.id = 0;
            mapInfo.thumbnail = await capture.CaptureToBinary();
            mapInfo.data = mapData;
            return mapInfo;
        }

        public static MapInfo ConvertJsonToMapInfo(string jsondata)
        {
            MapInfoRaw mapInfoRaw = JsonConvert.DeserializeObject<MapInfoRaw>(jsondata);
            
            return ConvertMapInfoRawToMapInfo(mapInfoRaw);
        }
        
        private static MapInfo ConvertMapInfoRawToMapInfo(MapInfoRaw mapInfoRaw)
        {
            MapInfo mapInfo = new MapInfo();
            
            mapInfo.id = mapInfoRaw.id;
            mapInfo.thumbnailUrl = mapInfoRaw.thumbnailUrl;

            mapInfo.data = new MapData();
            mapInfo.data.themeId = mapInfoRaw.theme;
            
            mapInfo.data.mapObjectList = new List<MapData.MapObjectData>();

            MapInfoRaw.MapObjectData[] mapObjectData;
            if (mapInfoRaw.mapObjectResDTO != null)
            {
                mapObjectData = mapInfoRaw.mapObjectResDTO;    
            }
            else if (mapInfoRaw.customMapObjects != null)
            {
                mapObjectData = mapInfoRaw.customMapObjects;
            }
            else
            {
                throw new ArgumentException($"ConvertMapInfoRawToMapInfo: {mapInfoRaw}");
            }
            
            foreach (var data in mapObjectData)
            {
                MapData.MapObjectData mapObject = new MapData.MapObjectData();
                mapObject.modelId =  data.modelId.ToString();
                mapObject.position = new SerializedVector3(data.positionX, data.positionY, data.positionZ);
                mapObject.rotation =
                    new SerializedQuaternion(data.rotationW, data.rotationX, data.rotationY, data.rotationZ);
                mapInfo.data.mapObjectList.Add(mapObject);
            }

            return mapInfo;
        }
        public static string ConvertMapInfoToJson(MapInfo mapInfo)
        {
            MapInfoRaw mapInfoRaw = new MapInfoRaw();

            mapInfoRaw.id = mapInfo.id;
            mapInfoRaw.thumbnailUrl = mapInfo.thumbnailUrl;
            mapInfoRaw.theme = mapInfo.data.themeId;

            mapInfoRaw.objects = new MapInfoRaw.MapObjectData[mapInfo.data.mapObjectList.Count];
            int i = 0;
            foreach (var data in mapInfo.data.mapObjectList)
            {
                MapInfoRaw.MapObjectData mapObject = new MapInfoRaw.MapObjectData();
                mapObject.modelId = long.Parse(data.modelId);
                mapObject.positionX = data.position.x;
                mapObject.positionY = data.position.y;
                mapObject.positionZ = data.position.z;
                mapObject.rotationW = data.rotation.w;
                mapObject.rotationX = data.rotation.x;
                mapObject.rotationY = data.rotation.y;
                mapObject.rotationZ = data.rotation.z;
                mapInfoRaw.objects[i++] = mapObject;
            }

            return JsonConvert.SerializeObject(mapInfoRaw);
        }

        public static List<MapInfo> ConvertJsonToMapList(string jsondata)
        {
            List<MapInfo> mapInfos = new List<MapInfo>();
            MapInfoRaw[] mapInfosRaw = JsonConvert.DeserializeObject<MapInfoRaw[]>(jsondata);

            foreach (var mapInfoRaw in mapInfosRaw)
            {
                mapInfos.Add(ConvertMapInfoRawToMapInfo(mapInfoRaw));
            }

            return mapInfos;
        }
    }
    
    
    [Serializable]
    class MapInfoRaw
    {
        public string creatorId = null;
        public long id = 0;
        public string thumbnailUrl = null;
        public string theme = "";

        public class MapObjectData
        {
            public long modelId;
            public double positionX;
            public double positionY;
            public double positionZ;
            public double rotationW;
            public double rotationX;
            public double rotationY;
            public double rotationZ;
        }

        public MapObjectData[] mapObjectResDTO = null;
        public MapObjectData[] customMapObjects = null;
        public MapObjectData[] objects = null;

        public bool ShouldSerializeid()
        {
            return id != null;
        }

        public bool ShouldSerializecreatorId()
        {
            return !String.IsNullOrEmpty(creatorId);
        }

        public bool ShouldSerializemapObjectResDTO()
        {
            return mapObjectResDTO != null;
        }

        public bool ShouldSerializecustomMapObjects()
        {
            return customMapObjects != null;
        }

        public bool ShouldSerializeobjects()
        {
            return objects != null;
        }
    }
}