using System;
using Newtonsoft.Json;

namespace Map
{
    public class MapConverter
    {
        public static MapConcrete ConvertMapInfoToMapConcrete(MapInfo info)
        {
            MapConcrete mapConcrete = new MapConcrete();
            MapData mapData = ConvertJsonToMapData(info.data);
            foreach (var mapObjectData in mapData.mapObjectList)
            {
                mapConcrete.AddMapObject(mapObjectData.position, mapObjectData.rotation, mapObjectData);
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