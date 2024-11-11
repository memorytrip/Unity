using System;
using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Map
{
    /**
     * MapInfo <-> MapConcrete 변환
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
        

        public static MapInfo ConvertMapConcreteToMapInfo(MapConcrete mapConcrete)
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
            mapInfo.data = mapData;
            return mapInfo;
        }

        public static MapInfo ConvertJsonToMapInfo(string jsondata)
        {
            MapInfo mapInfo = new MapInfo();
            MapInfoRaw mapInfoRaw = JsonConvert.DeserializeObject<MapInfoRaw>(jsondata);
            
            mapInfo.id = mapInfoRaw.id;
            mapInfo.thumbnail = mapInfo.thumbnail;

            mapInfo.data = new MapData();
            mapInfo.data.themeId = mapInfoRaw.data.themeId;
            
            mapInfo.data.mapObjectList = new List<MapData.MapObjectData>();
            MapInfoRaw.MapData mapData = mapInfoRaw.data;
            MapInfoRaw.MapData.MapObjectData[] mapObjectData = mapData.mapObjectList;
            foreach (var data in mapObjectData)
            {
                MapData.MapObjectData mapObject = new MapData.MapObjectData();
                mapObject.modelId = data.modelId;
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
            mapInfoRaw.thumbnailUrl = mapInfo.thumbnail;

            mapInfoRaw.data = new MapInfoRaw.MapData();
            mapInfoRaw.data.themeId = mapInfo.data.themeId;

            mapInfoRaw.data.mapObjectList = new MapInfoRaw.MapData.MapObjectData[mapInfo.data.mapObjectList.Count];
            int i = 0;
            foreach (var data in mapInfo.data.mapObjectList)
            {
                MapInfoRaw.MapData.MapObjectData mapObject = new MapInfoRaw.MapData.MapObjectData();
                mapObject.modelId = data.modelId;
                mapObject.positionX = data.position.x;
                mapObject.positionY = data.position.y;
                mapObject.positionZ = data.position.z;
                mapObject.rotationW = data.rotation.w;
                mapObject.rotationX = data.rotation.x;
                mapObject.rotationY = data.rotation.y;
                mapObject.rotationZ = data.rotation.z;
                mapInfoRaw.data.mapObjectList[i++] = mapObject;
            }

            return JsonConvert.SerializeObject(mapInfoRaw);
        }
    }
    
    
    [Serializable]
    class MapInfoRaw
    {
        public string id;
        public string thumbnailUrl;
        public class MapData
        {
            public string themeId;

            public class MapObjectData
            {
                public string modelId;
                public float positionX;
                public float positionY;
                public float positionZ;
                public float rotationW;
                public float rotationX;
                public float rotationY;
                public float rotationZ;
            }

            public MapObjectData[] mapObjectList;
        }

        public MapData data;
    }
}