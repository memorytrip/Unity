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
        public string id;
        public Texture2D thumbnail;
        public string data;

        
    }
}