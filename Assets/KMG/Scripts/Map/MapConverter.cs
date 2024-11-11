using System.Collections.Generic;
using Common;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace Map
{
    /**
     * MapInfo <-> MapConcrete 변환
     */
    public class MapConverter
    {
        public static async UniTask<MapConcrete> ConvertMapInfoToMapConcrete(MapInfo info)
        {
            // MapData mapData = ConvertJsonToMapData(info.data);
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
        
        private static MapData ConvertJsonToMapData(string data)
        {
            return JsonConvert.DeserializeObject<MapData>(data);
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
            // mapInfo.data = ConvertMapDataToJson(mapData);
            mapInfo.data = mapData;
            return mapInfo;
        }

        private static string ConvertMapDataToJson(MapData mapData)
        {
            return JsonConvert.SerializeObject(mapData);
        }
    }
}