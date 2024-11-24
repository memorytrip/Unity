using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Map
{
    /**
     * MapData만 담고 있는 객체. MapConcrete와 달리 게임오브젝트로 구체화되지 않음.
     */
    public class MapInfo
    {
        public static MapInfo GetDefaultMap()
        {
            MapInfo res = new MapInfo();
            // res.data = "{\"themeId\":\"0\",\"mapObjectList\":[]}";
            res.data = new MapData();
            res.type = MapType.Default;
            res.data.themeId = "0";
            res.data.mapObjectList = new List<MapData.MapObjectData>();
            return res;
        }

        public enum MapType
        {
            Default,
            Custom
        }

        public MapType type;
        public long id;
        public byte[] thumbnail;
        public string thumbnailUrl;
        public MapData data;
    }
    
    public struct MapId
    {
        public MapId(MapInfo.MapType mapType, long mapId)
        {
            this.mapId = mapId;
            this.mapType = mapType;
        }
        public MapInfo.MapType mapType;
        public long mapId;
    }
}