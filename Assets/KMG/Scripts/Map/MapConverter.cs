using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Map
{
    /**
     * MapInfo <-> MapConcrete 변환
     * TODO: convert과정 구현하기
     */
    public class MapConverter
    {
        public static MapConcrete ConvertMapInfoToMapConcrete(MapInfo info)
        {
            MapData mapData = ConvertJsonToMapData(info.data);
            
            // MapConcrete 초기화
            MapConcrete mapConcrete = new MapConcrete(info);
            
            // MapConcrete에 MapObject 추가
            foreach (var mapObjectData in mapData.mapObjectList)
            {
                Model model = ModelManager.Instance.Find(mapObjectData);
                mapConcrete.AddMapObject(mapObjectData.position, mapObjectData.rotation, model);
            }
            
            return mapConcrete;
        }
        
        private static MapData ConvertJsonToMapData(string data)
        {
            return JsonConvert.DeserializeObject<MapData>(data);
        }

        public static MapInfo ConvertMapConcreteToMapInfo(MapConcrete mapConcrete)
        {
            MapData mapData = new MapData();
            mapData.themeId = mapConcrete.themeId;
            foreach (var mapObject in mapConcrete.mapObjects)
            {
                MapData.MapObjectData objData = new MapData.MapObjectData();
                objData.modelId = mapObject.GetModel().id;
                objData.position = mapObject.transform.position;
                objData.rotation = mapObject.transform.rotation;
                mapData.mapObjectList.Add(objData);
            }

            //TODO: mapInfo.id, mapInfo.thmbnail 채우기
            MapInfo mapInfo = new MapInfo();
            mapInfo.data = ConvertMapDataToJson(mapData);
            return mapInfo;
        }

        private static string ConvertMapDataToJson(MapData mapData)
        {
            return JsonConvert.SerializeObject(mapData);
        }
    }
}