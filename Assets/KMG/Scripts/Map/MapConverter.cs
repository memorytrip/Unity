using System;
using Newtonsoft.Json;

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
                MapObjectModel model = ModelManager.Find(mapObjectData);
                mapConcrete.AddMapObject(mapObjectData.position, mapObjectData.rotation, model);
            }
            
            throw new NotImplementedException();
        }

        public static MapInfo ConvertMapConcreteToMapInfo(MapConcrete mapConcrete)
        {
            throw new NotImplementedException();
        }

        private static MapData ConvertJsonToMapData(string data)
        {
            return JsonConvert.DeserializeObject<MapData>(data);
        }
    }
}