using System;
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
            res.data =
                "{\"themeId\":\"0\",\"mapObjectList\":[]}";
            return res;
        }
        public string formatVersion = "0.0.1";
        public string id;
        public string thumbnail;
        public string data;
    }
}